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
    public class DoctorController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DoctorController()
        {

            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44353/api/");
        }
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
            //use it to pass along to the WebApi.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }


        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>




        // GET: Doctor/List
        public ActionResult List()
        {
            //objective: communicate with doctor data api to retrieve a list of doctors
            //curl https://localhost:44353/api/doctordata/listdoctors

            string url = "doctordata/listdoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DoctorDto> doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;

            //Debug.WriteLine("Number of Doctors: ");
            //Debug.WriteLine(doctors.Count());

            return View(doctors);
        }


        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with doctor data api to retrieve one doctor
            //curl https://localhost:44353/api/doctordata/finddoctor/{id}

            DetailsDoctor ViewModel = new DetailsDoctor();

            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;

            //Debug.WriteLine("Doctor Recieved: ");
            //Debug.WriteLine(SelectedDoctor.DoctorFirstName +" "+SelectedDoctor.DoctorLastName);

            ViewModel.SelectedDoctor = SelectedDoctor;

            url = "proceduredata/listproceduresfordoctor/" + id;
            response = client.GetAsync(url).Result;
            //Showcase information about procedures related to this doctor
            //send a request to gather infromation about procedures related to a particular doctor id

            IEnumerable<ProcedureDto> RelatedProcedures = response.Content.ReadAsAsync<IEnumerable<ProcedureDto>>().Result;

            ViewModel.RelatedProcedures = RelatedProcedures;

            return View(ViewModel);
        }


        // GET: Doctor/Error
        public ActionResult Error()
        {
            return View();
        }


        // GET: Doctor/New
        public ActionResult New()
        {
            return View();
        }


        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            GetApplicationCookie();//get token credentials

            string url = "doctordata/adddoctor";

            string jsonpayload = jss.Serialize(doctor);

            //Debug.WriteLine("Payload: ");
            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            //Debug.WriteLine("Content: ");
            //Debug.WriteLine(content);

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Debug.WriteLine("Response: ");
            //Debug.WriteLine(response);
            //Debug.WriteLine("Status Code: ");
            //Debug.WriteLine(response.IsSuccessStatusCode);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Doctor/Edit/5
        public ActionResult Edit(int id)
        {
            //The existing doctor information
            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(SelectedDoctor);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Doctor doctor)
        {
            //objective: update the details of a doctor already in our system
            //curl -H "Content-Type:application/json" -d @doctor.json https://localhost:44393/api/doctordata/updatedoctor/{id}

            GetApplicationCookie();//get token credentials

            string url = "doctordata/updatedoctor/" + id;

            //Converting form data into JSON object
            string jsonpayload = jss.Serialize(doctor);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload

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


        // GET: Doctor/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //The existing doctor information
            string url = "doctordata/finddoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(SelectedDoctor);
        }


        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            GetApplicationCookie();//get token credentials

            string url = "doctordata/deletedoctor/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload
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
