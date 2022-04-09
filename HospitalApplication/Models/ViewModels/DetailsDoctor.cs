using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class DetailsDoctor
    {
        public DoctorDto SelectedDoctor { get; set; }
        public IEnumerable<ProcedureDto> RelatedProcedures { get; set; }

    }
}