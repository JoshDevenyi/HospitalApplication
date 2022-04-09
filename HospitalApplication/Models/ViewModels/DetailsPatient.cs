using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class DetailsPatient
    {
        public PatientDto SelectedPatient { get; set; }
        public IEnumerable<ProcedureDto> RelatedProcedures { get; set; }
    }
}