using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class NewProcedure
    {
        //Room options
        public IEnumerable<RoomDto> RoomOptions { get; set; }

        //Doctor options
        public IEnumerable<DoctorDto> DoctorOptions { get; set; }

        //Patient options
        public IEnumerable<PatientDto> PatientOptions { get; set; }
    }
}