using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class UpdateProcedure
    {
        //Stores info to be presented to /Procedure/Update/id

        //Existing procedure info

        public ProcedureDto SelectedProcedure { get; set; }

        //Room options

        public IEnumerable<RoomDto> RoomOptions { get; set; }
        
        //Doctor options
        public IEnumerable<DoctorDto> DoctorOptions { get; set; }
        
        //Patient options
        public IEnumerable<PatientDto> PatientOptions { get; set; }


    }
}