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
    public class ProcedureDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all procedures in the database
        /// </summary>
        /// <returns>
        /// CONTENT: All procedures in the database
        /// </returns>
        /// <example>
        /// GET: api/ProcedureData/ListProcedures
        /// </example>

        [HttpGet]
        public IEnumerable<ProcedureDto> ListProcedures()
        {
            List<Procedure> Procedures = db.Procedures.ToList();
            List<ProcedureDto> ProcedureDtos = new List<ProcedureDto>();

            Procedures.ForEach(p => ProcedureDtos.Add(new ProcedureDto()
            {
                ProcedureId = p.ProcedureId,
                ProcedureName = p.ProcedureName,
                DoctorFirstName = p.Doctor.DoctorFirstName,
                DoctorLastName = p.Doctor.DoctorLastName,
                PatientFirstName = p.Patient.PatientFirstName,
                PatientLastName = p.Patient.PatientLastName,
                RoomNumber = p.Room.RoomNumber,
                Duration = p.Duration,
                Date = p.Date,
                Time = p.Time

            }));

            return ProcedureDtos;
        }

        /// <summary>
        /// Gather information for all procedures related to a particular doctor id
        /// </summary>
        /// <returns>
        /// CONTENT: All procedures in the database related to the desired doctor
        /// </returns>
        /// <param name="id">Doctor Id</param>
        /// <example>
        /// GET: api/ProcedureData/ListProceduresForDoctor/3
        /// </example>

        [HttpGet]
        public IEnumerable<ProcedureDto> ListProceduresForDoctor(int id)
        {
            List<Procedure> Procedures = db.Procedures.Where(p=>p.ProcedureDoctor==id).ToList();
            List<ProcedureDto> ProcedureDtos = new List<ProcedureDto>();

            Procedures.ForEach(p => ProcedureDtos.Add(new ProcedureDto()
            {
                ProcedureId = p.ProcedureId,
                ProcedureName = p.ProcedureName,
                DoctorFirstName = p.Doctor.DoctorFirstName,
                DoctorLastName = p.Doctor.DoctorLastName,
                PatientFirstName = p.Patient.PatientFirstName,
                PatientLastName = p.Patient.PatientLastName,
                RoomNumber = p.Room.RoomNumber,
                Duration = p.Duration,
                Date = p.Date,
                Time = p.Time

            }));

            return ProcedureDtos;
        }

        /// <summary>
        /// Returns all procedures in the database
        /// </summary>
        /// <returns>
        /// CONTENT: All procedures in the database
        /// </returns>
        /// <param name="id">Patient ID</param>
        /// <example>
        /// GET: api/ProcedureData/ListProcedures/3
        /// </example>

        [HttpGet]
        public IEnumerable<ProcedureDto> ListProceduresForPatient(int id)
        {
            List<Procedure> Procedures = db.Procedures.Where(p => p.ProcedurePatient == id).ToList();
            List<ProcedureDto> ProcedureDtos = new List<ProcedureDto>();

            Procedures.ForEach(p => ProcedureDtos.Add(new ProcedureDto()
            {
                ProcedureId = p.ProcedureId,
                ProcedureName = p.ProcedureName,
                DoctorFirstName = p.Doctor.DoctorFirstName,
                DoctorLastName = p.Doctor.DoctorLastName,
                PatientFirstName = p.Patient.PatientFirstName,
                PatientLastName = p.Patient.PatientLastName,
                RoomNumber = p.Room.RoomNumber,
                Duration = p.Duration,
                Date = p.Date,
                Time = p.Time

            }));

            return ProcedureDtos;
        }

        /// <summary>
        /// Return a procedure based on a given ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A procedure from the Database with the matching ID
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Procedure</param>
        /// <example>
        /// GET: api/ProcedureData/FindProcedure/5
        /// </example>
        [ResponseType(typeof(Procedure))]
        [HttpGet]
        public IHttpActionResult FindProcedure(int id)
        {
            Procedure procedure = db.Procedures.Find(id);
            ProcedureDto ProcedureDto = new ProcedureDto()
            {
                ProcedureId = procedure.ProcedureId,
                ProcedureName = procedure.ProcedureName,
                DoctorFirstName = procedure.Doctor.DoctorFirstName,
                DoctorLastName = procedure.Doctor.DoctorLastName,
                ProcedureDoctor = procedure.ProcedureDoctor,
                PatientFirstName = procedure.Patient.PatientFirstName,
                PatientLastName = procedure.Patient.PatientLastName,
                ProcedurePatient = procedure.ProcedurePatient,
                RoomNumber = procedure.Room.RoomNumber,
                ProcedureRoom = procedure.ProcedureRoom,
                Duration = procedure.Duration,
                Date = procedure.Date,
                Time = procedure.Time
            };
            if (procedure == null)
            {
                return NotFound();
            }

            return Ok(ProcedureDto);
        }

        /// <summary>
        /// Updates a specified Procedure in the system with a POST Data input
        /// </summary>
        /// <param name="id">Represents the Procedures primary key id</param>
        /// <param name="procedure">JSON FORM DATA of a Procedure</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ProcedureData/UpdateProcedure/5
        /// FORM DATA: Procedure JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IHttpActionResult UpdateProcedure(int id, Procedure procedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != procedure.ProcedureId)
            {
                return BadRequest();
            }

            db.Entry(procedure).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcedureExists(id))
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
        /// Adds a new Procedure to the system
        /// </summary>
        /// <param name="procedure">JSON FORM DATA of a Procedure</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Procedure ID, Procedure Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ProcedureData/AddProcedure
        /// FORM DATA: Procedure JSON Object
        /// </example>
        [ResponseType(typeof(Procedure))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddProcedure(Procedure procedure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Procedures.Add(procedure);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = procedure.ProcedureId }, procedure);
        }


        /// <summary>
        /// Deletes a Procedure from the system by a provided id.
        /// </summary>
        /// <param name="id">A Procedure's Primary Key Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ProcedureData/DeleteProcedure/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Procedure))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteProcedure(int id)
        {
            Procedure procedure = db.Procedures.Find(id);
            if (procedure == null)
            {
                return NotFound();
            }

            db.Procedures.Remove(procedure);
            db.SaveChanges();

            return Ok("Procedure Deleted");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProcedureExists(int id)
        {
            return db.Procedures.Count(e => e.ProcedureId == id) > 0;
        }
    }
}