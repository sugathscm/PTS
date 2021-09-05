using PTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PTS.UI.Models
{
    public class PatientMedicalHistoryViewModel : PatientMedicalHistory
    {
        public List<CATScore> CATScoreList { get; set; }

        public string PatientName { get; set; }
        public int GenderId { get; set; }
        public int TitleId { get; set; }
        public int MaritialStatusId { get; set; }
        public string Designation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
    }
}