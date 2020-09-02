using PTS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PTS.BAL
{
    public class GenericService
    {
        public List<T> GetList<T>() where T : class
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                return entities.Set<T>().ToList();
            }
        }

        public void SaveOrUpdate<T>(T t, int id) where T : class
        {
            using (PTS_AmithaEntities entities = new PTS_AmithaEntities())
            {
                if (id == 0)
                {
                    entities.Set<T>().Add(t);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

    }
}
