using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApplication.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public int StaffNumber { get; set; }
        public string Department { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorEmail { get; set; }
    }

    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public int StaffNumber { get; set; }
        public string Department { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorEmail { get; set; }

    }

}