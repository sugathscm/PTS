using PTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.BAL
{
    public class PatientService
    {
        public List<Patient> GetPatientList()
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                return entities.Patients.OrderBy(d => d.FirstName).ToList();
            }
        }

        public Patient GetPatientById(int? id)
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                return entities.Patients
                    .Include("Title")
                    .Include("Gender")
                    .Include("MaritialStatu")
                    .Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(Patient patient)
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                if (patient.Id == 0)
                {
                    entities.Patients.Add(patient);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(patient).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

        public int? GetMaxCodeNumber(string prefix)
        {
            int? number = 0;
            try
            {
                using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
                {
                    var lastRecord = entities.Patients.Where(s => s.CodePrefix == prefix).OrderByDescending(d => d.CodeNumber).Take(1).FirstOrDefault();

                    if (lastRecord == null)
                    {
                        number = -1;
                    }
                    else
                    {
                        number = lastRecord.CodeNumber;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return number;

        }

    }

}
