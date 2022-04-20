using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalApplication.Models;
using HospitalApplication.Models.ViewModels;
using System.Web.Script.Serialization;

namespace HospitalApplication.Controllers
{
    public class PatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>

        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Patient/List
        public ActionResult List()
        {
            //Communicate with PatientData API to initiate a list of patients

            string url = "patientdata/listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;

            //Debug.WriteLine("Number of patients received: ");
            //Debug.WriteLine(patients.Count());

            return View(patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            //Communicate with PatientData API to retrieve a patient

            DetailsPatient ViewModel = new DetailsPatient();

            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            Debug.WriteLine("Patient Received: ");
            Debug.WriteLine(SelectedPatient.PatientFirstName);

            ViewModel.SelectedPatient = SelectedPatient;


            //Send a request to gather information about procedures related to a patient ID 
            url = "proceduredata/listproceduresforpatient/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ProcedureDto> RelatedProcedures = response.Content.ReadAsAsync<IEnumerable<ProcedureDto>>().Result; ;

            ViewModel.RelatedProcedures = RelatedProcedures;

            return View(ViewModel);
        }

        // GET: Patient/Delete/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Patient/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            GetApplicationCookie();
            Debug.WriteLine("The json payload is: ");

            //add a new patient into the system using API data

            string url = "patientdata/addpatient";

            string jsonpayload = jss.Serialize(patient);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(SelectedPatient);
        }

        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patient patient)
        {
            GetApplicationCookie();
            string url = "patientdata/updatepatient/" + id;

            string jsonpayload = jss.Serialize(patient);

            Debug.WriteLine("Payload: ");
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            Debug.WriteLine("Content: ");
            Debug.WriteLine(content);

            HttpResponseMessage response = client.PostAsync(url, content).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: patient/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            //The existing patient information
            string url = "patientdata/findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(SelectedPatient);
        }

        // POST: patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            GetApplicationCookie();
            string url = "patientdata/deletepatient/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(id);
            Debug.WriteLine(response);
            Debug.WriteLine(response.IsSuccessStatusCode);

            //Checking that response was successful
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}