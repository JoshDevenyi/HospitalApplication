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
    public class RoomController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static RoomController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44353/api/roomdata/");
        }


        // GET: Room/List
        public ActionResult List()
        {
            //objective: communicate with room data api to retrieve a list of rooms
            //curl https://localhost:44353/api/roomdata/listrooms

            string url = "listrooms";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RoomDto> rooms = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;

            return View(rooms);
        }

        // GET: Room/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with room data api to retrieve one room
            //curl https://localhost:44353/api/roomdata/findroom/{id}

            string url = "findroom/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            RoomDto SelectedRoom = response.Content.ReadAsAsync<RoomDto>().Result;

            return View(SelectedRoom);
        }

        // GET: Room/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Room/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Room/Create
        [HttpPost]
        public ActionResult Create(Room room)
        {

            string url = "addroom";

            string jsonpayload = jss.Serialize(room);

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
   

        // GET: Room/Edit/5
        public ActionResult Edit(int id)
        {
            //The existing room information
            string url = "findroom/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RoomDto SelectedRoom = response.Content.ReadAsAsync<RoomDto>().Result;
            return View(SelectedRoom);
        }

        // POST: Room/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Room room)
        {
            //objective: update the details of a room already in our system
            //curl -H "Content-Type:application/json" -d @room.json https://localhost:44393/api/roomdata/updateroom/{id}
            string url = "updateroom/" + id;

            string jsonpayload = jss.Serialize(room);

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

        // GET: Room/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "findroom/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RoomDto SelectedRoom = response.Content.ReadAsAsync<RoomDto>().Result;
            return View(SelectedRoom);
        }

        // POST: Room/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "deleteroom/" + id;

            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json"; //Specifies that we are sending JSON information as part of the payload
            HttpResponseMessage response = client.PostAsync(url, content).Result;

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
