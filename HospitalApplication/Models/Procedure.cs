using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApplication.Models
{
    public class Procedure
    {
        [Key]
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; }

        [ForeignKey("Doctor")]
        public int ProcedureDoctor { get; set; }
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("Patient")]
        public int ProcedurePatient { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Room")]
        public int ProcedureRoom { get; set; }
        public virtual Room Room { get; set; }
        
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
    }

    public class ProcedureDto
    {
        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public int ProcedureDoctor { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int ProcedurePatient { get; set; }
        public int RoomNumber { get; set; }
        public int ProcedureRoom{ get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
    }
}