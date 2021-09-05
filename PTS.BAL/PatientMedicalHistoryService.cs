using PTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.BAL
{
    public class PatientMedicalHistoryService
    {
        public List<PatientMedicalHistory> GetPatientMedicalHistoryList()
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                return entities.PatientMedicalHistories
                    .Include("mMRCGrade")
                    .Include("SmokingHistory")
                    .OrderBy(d => d.DateCreated).ToList();
            }
        }

        public List<PatientMedicalHistory> GetPatientMedicalHistoryListByPatient(int patientId)
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                return entities.PatientMedicalHistories
                    .Include("mMRCGrade")
                    .Include("SmokingHistory")
                    .Where(h => h.PatientId == patientId)
                    .OrderBy(d => d.DateCreated).ToList();
            }
        }

        public PatientMedicalHistory GetPatientMedicalHistoryById(int? id)
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                return entities.PatientMedicalHistories
                    .Include("mMRCGrade")
                    .Include("SmokingHistory")
                    .Where(s => s.Id == id).SingleOrDefault();
            }
        }
    }
}
