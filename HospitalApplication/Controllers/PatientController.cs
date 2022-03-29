using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalApplication.Models;
using System.Web.Script.Serialization;

namespace HospitalApplication.Controllers
{
    public class PatientController : Controller
    {
            private static readonly HttpClient client;
            private JavaScriptSerializer jss = new JavaScriptSerializer();

            static PatientController()
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:44353/api/patientdata/");
            }

            // GET: Patient/List
            public ActionResult List()
            {
                //Communicate with PatientData API to initiate a list of patients

                string url = "listpatients";
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
                string url = "findpatient/" + id;
                HttpResponseMessage response = client.GetAsync(url).Result;

                Debug.WriteLine("The response code is ");
                Debug.WriteLine(response.StatusCode);

                PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;
                Debug.WriteLine("Patient Received: ");
                Debug.WriteLine(selectedpatient.PatientFirstName);

                return View(selectedpatient);
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
            Debug.WriteLine("The json payload is: ");

            //add a new patient into the system using API data

            string url = "addpatient";

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
            string url = "findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(SelectedPatient);
        }

        // POST: Patient/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patient patient)
        {
            string url = "updatepatient/" + id;

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
            string url = "findpatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto SelectedPatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(SelectedPatient);
        }

        // POST: patient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "deletepatient/" + id;

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
