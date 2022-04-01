using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalApplication.Models;

namespace HospitalApplication.Controllers
{
    public class RoomDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns All Rooms in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: All the Rooms in the database
        /// </returns>
        /// <example>
        /// GET: api/RoomData/ListRooms
        /// </example>
        [HttpGet]
        public IEnumerable<RoomDto> ListRooms()
        {
            List<Room> Rooms = db.Rooms.ToList();
            List<RoomDto> RoomDtos = new List<RoomDto>();

            Rooms.ForEach(r => RoomDtos.Add(new RoomDto()
            {
                RoomId = r.RoomId,
                RoomPurpose = r.RoomPurpose,
                RoomNumber = r.RoomNumber,
                Floor = r.Floor,
                Building = r.Building
            }));

            return RoomDtos;
        }


        /// <summary>
        /// Returns a specific room in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An room in the system that corresponds to the provided primary key. 
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the room</param>
        /// <example>
        /// GET: api/RoomData/FindRoom/5
        /// </example>
        [ResponseType(typeof(Room))]
        [HttpGet]
        public IHttpActionResult FindRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            RoomDto RoomDto = new RoomDto()
            {
                RoomId = room.RoomId,
                RoomPurpose = room.RoomPurpose,
                RoomNumber = room.RoomNumber,
                Floor = room.Floor,
                Building = room.Building
            };

            if (room == null)
            {
                return NotFound();
            }

            return Ok(RoomDto);
        }

        /// <summary>
        /// Updates a specified room in the system with a POST Data input
        /// </summary>
        /// <param name="id">Represents the Rooms primary key id</param>
        /// <param name="room">JSON FORM DATA of a Room</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/RoomData/UpdateRoom/5
        /// FORM DATA: Room JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRoom(int id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.RoomId)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new Room to the system
        /// </summary>
        /// <param name="room">JSON FORM DATA of a Room</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Room ID, Room Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/RoomData/AddRoom
        /// FORM DATA: Room JSON Object
        /// </example>
        [ResponseType(typeof(Room))]
        [HttpPost]
        public IHttpActionResult AddRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = room.RoomId }, room);
        }

        /// <summary>
        /// Deletes an Room from the system by a provided id.
        /// </summary>
        /// <param name="id">A Rooms Primary Key Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/RoomData/DeleteRoom/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Room))]
        [HttpPost]
        public IHttpActionResult DeleteRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            db.Rooms.Remove(room);
            db.SaveChanges();

            return Ok(room);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.RoomId == id) > 0;
        }
    }
}