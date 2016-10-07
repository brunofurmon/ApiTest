using ApiTest.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace ApiTest.Daos
{
    public interface IAbstractDao<T> where T : AbstractModel
    {
        /// GET: api/T
        List<T> List();
        /// GET: api/T/5
        T Get(long id);
        /// PUT: api/Ts/5
        T Update(long id, T bean);
        /// POST: api/Ts
        T Create(T bean);
        /// DELETE: api/Ts/5
        T Delete(long id);
    }

    public class AbstractDao<T>: DbContext, IAbstractDao<T> where T: AbstractModel
    {
        public AbstractDao() : base("name=TServiceContext")
        {
        }

        private DbSet<T> db { get; set; }

        // GET: api/T
        public List<T> List()
        {
            return db.ToList();
        }

        // GET: api/T/5
        public T Get(long id)
        {
            T bean = db.Find(id);
            if (bean != null)
            {
                return bean;
            }
            return null;
        }

        // PUT: api/Ts/5
        public T Update(long id, T bean)
        {
            this.Entry(bean).State = EntityState.Modified;
  
            try
            {
                this.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (Exists(id))
                {
                    throw;
                }
            }
            return bean;
        }

        // POST: api/Ts
        public T Create(T bean)
        {
            db.Add(bean);
            this.SaveChanges();

            return bean;
        }

        // DELETE: api/Ts/5
        public T Delete(long id)
        {
            T bean = db.Find(id);
            if (bean == null)
            {
                return null;
            }

            db.Remove(bean);
            this.SaveChanges();

            return bean;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Exists(long id)
        {
            return db.Count(e => e.Id == id) > 0;
        }
    }
}