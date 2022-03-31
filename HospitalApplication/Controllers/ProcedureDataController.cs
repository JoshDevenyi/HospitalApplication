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

        // GET: api/ProcedureData/List
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

        // GET: api/ProcedureData/FindProcedure/1
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

        // POST: api/ProcedureData/UpdateProcedure/1
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/ProcedureData/AddProcedure
        [ResponseType(typeof(Procedure))]
        [HttpPost]
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

        // POST: api/ProcedureData/DeleteProcedure/1
        [ResponseType(typeof(Procedure))]
        [HttpPost]
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