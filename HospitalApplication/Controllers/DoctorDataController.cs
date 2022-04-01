using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Web.Http.Description;
using HospitalApplication.Models;

namespace HospitalApplication.Controllers
{
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns All Doctors in the system.
        /// </summary>
        /// <returns>
        /// CONTENT: All the Doctors in the database
        /// </returns>
        /// <example>
        /// GET: api/DoctorData/ListDoctors
        /// </example>
        [HttpGet]
        public IEnumerable<DoctorDto> ListDoctors()
        {
            List<Doctor> Doctors = db.Doctors.ToList();
            List<DoctorDto> DoctorDtos = new List<DoctorDto>();

            Doctors.ForEach(d => DoctorDtos.Add(new DoctorDto()
            {
                DoctorId = d.DoctorId,
                DoctorFirstName = d.DoctorFirstName,
                DoctorLastName = d.DoctorLastName,
                StaffNumber = d.StaffNumber,
                Department = d.Department,
                DoctorPhone = d.DoctorPhone,
                DoctorEmail = d.DoctorEmail
            }));

            return DoctorDtos;

        }

        /// <summary>
        /// Returns a specific doctor in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An doctor in the system that corresponds to the provided primary key. 
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the doctor</param>
        /// <example>
        /// GET: api/DoctorData/FindDoctor/5
        /// </example>
        [ResponseType(typeof(Doctor))]
        [HttpGet]
        public IHttpActionResult FindDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            DoctorDto DoctorDto = new DoctorDto()
            {
                DoctorId = doctor.DoctorId,
                DoctorFirstName = doctor.DoctorFirstName,
                DoctorLastName = doctor.DoctorLastName,
                StaffNumber = doctor.StaffNumber,
                Department = doctor.Department,
                DoctorPhone = doctor.DoctorPhone,
                DoctorEmail = doctor.DoctorEmail
            };
            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(DoctorDto);
        }


        /// <summary>
        /// Updates a specified doctor in the system with a POST Data input
        /// </summary>
        /// <param name="id">Represents the Doctors primary key id</param>
        /// <param name="doctor">JSON FORM DATA of a Doctor</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/UpdateDoctor/5
        /// FORM DATA: Doctor JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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
        /// Adds a new Doctor to the system
        /// </summary>
        /// <param name="doctor">JSON FORM DATA of a Doctor</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Doctor ID, Doctor Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/AddDoctor
        /// FORM DATA: Doctor JSON Object
        /// </example>
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult AddDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorId }, doctor);
        }


        /// <summary>
        /// Deletes an Doctor from the system by a provided id.
        /// </summary>
        /// <param name="id">A Doctors Primary Key Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/DoctorData/DeleteDoctor/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {

            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.DoctorId == id) > 0;
        }
    }
}