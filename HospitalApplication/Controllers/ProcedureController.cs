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
    public class ProcedureController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProcedureController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/proceduredata/");
        }
        
        
        // GET: Procedure/List
        public ActionResult List()
        {
            //Communicate with ProcedureData API to pull a list of procedures

            string url = "listprocedures";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ProcedureDto> procedures = response.Content.ReadAsAsync<IEnumerable<ProcedureDto>>().Result;

            return View(procedures);
        }

        // GET: Procedure/Details/3
        public ActionResult Details(int id)
        {

            //Communicate with the ProcedureData API to collect info about a SINGLE procedure
            string url = "findprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ProcedureDto selectedprocedure = response.Content.ReadAsAsync<ProcedureDto>().Result;

            
            return View(selectedprocedure);
        }

        // GET: Procedure/Delete/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Procedure/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Procedure/Create
        [HttpPost]
        public ActionResult Create(Procedure procedure)
        {
            //Add a new procedure to the system

            string url = "addprocedure";

            string jsonpayload = jss.Serialize(procedure);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

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

        // GET: Procedure/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "findprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProcedureDto SelectedProcedure = response.Content.ReadAsAsync<ProcedureDto>().Result;
            return View(SelectedProcedure);
        }

        // POST: Procedure/Update/5
        [HttpPost]
        public ActionResult Update(int id, Procedure procedure)
        {
            string url = "updateprocedure/" + id;

            string jsonpayload = jss.Serialize(procedure);

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

        // GET: Procedure/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findprocedure/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProcedureDto SelectedProcedure = response.Content.ReadAsAsync<ProcedureDto>().Result;
            return View(SelectedProcedure);
        }

        // POST: Procedure/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "deleteprocedure/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //Confirm whether attempt was successfull 
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
