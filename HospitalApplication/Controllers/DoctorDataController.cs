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

        // GET: api/DoctorData/ListDoctors
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

        // GET: api/DoctorData/FindDoctor/5
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

        // PUT: api/DoctorData/UpdateDoctor/5
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


        // POST: api/DoctorData/AddDoctor
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


        // DELETE: api/DoctorData/DeleteDoctor/5
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            //Debug.WriteLine("The ID is: ");
            //Debug.WriteLine(id);

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