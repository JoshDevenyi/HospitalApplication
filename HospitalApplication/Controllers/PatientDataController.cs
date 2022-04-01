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
using System.Diagnostics;
using HospitalApplication.Models;

namespace HospitalApplication.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all Patients in the database
        /// </summary>
        /// <returns>
        /// CONTENT: All the Patients in the database
        /// </returns>
        /// <example>
        /// GET: api/PatientData/ListPatients
        /// </example>
        
        [HttpGet]
        public IQueryable<Patient> ListPatients()
        {
            return db.Patients;
        }

        /// <summary>
        /// Return a Patient based on a given ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Patient from the Database with the matching ID
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Patient</param>
        /// <example>
        /// GET: api/PatientData/FindPatient/5
        /// </example>
        [ResponseType(typeof(Patient))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        /// <summary>
        /// Updates a specified Patient in the system with a POST Data input
        /// </summary>
        /// <param name="id">Represents the Patients primary key id</param>
        /// <param name="Patient">JSON FORM DATA of a Patient</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/UpdatePatient/5
        /// FORM DATA: Patient JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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
        /// Adds a new Patient to the system
        /// </summary>
        /// <param name="patient">JSON FORM DATA of a Patient</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Patient ID, Patient Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/AddPatient
        /// FORM DATA: Patient JSON Object
        /// </example>
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientId }, patient);
        }

        /// <summary>
        /// Deletes a Patient from the system by a provided id.
        /// </summary>
        /// <param name="id">A Patient's Primary Key Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PatientData/DeletePatient/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok("Patient Deleted");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientId == id) > 0;
        }
    }
}