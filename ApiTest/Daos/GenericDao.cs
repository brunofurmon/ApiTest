using ApiTest.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace ApiTest.Daos
{
    public interface IGenericDao<T> where T : AbstractModel
    {
        /// GET: api/T
        List<T> List();
        /// GET: api/T/5
        T Get(int id);
        /// PUT: api/Ts/5
        T Update(T bean);
        /// POST: api/Ts
        T Create(T bean);
        /// DELETE: api/Ts/5
        T Delete(int id);
    }

    public class GenericDao<T> : DbContext, IGenericDao<T> where T : AbstractModel
    {
        public GenericDao() : base("name=ApiTest")
        {
        }

        public virtual DbSet<T> db { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>().Map(m =>
            {
                m.MapInheritedProperties();
            });

            base.OnModelCreating(modelBuilder);
        }

        public List<T> List()
        {
            if (db.Count() == 0)
            {
                return new List<T>();
            }
            return db.ToList();
        }

        public T Get(int id)
        {
            T bean = db.Find(id);
            if (bean != null)
            {
                return bean;
            }
            return null;
        }

        public T Update(T bean)
        {
            // Finds original bean before attaching
            T originalBean = db.Find(bean.Id);

            if (originalBean == null)
            {
                return null;
            }

            DbEntityEntry<T> entry = Entry(originalBean);
            entry.CurrentValues.SetValues(bean);
            try
            {
                this.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (Exists(bean.Id))
                {
                    throw;
                }
            }
            return bean;
        }

        public T Create(T bean)
        {
            db.Add(bean);
            this.SaveChanges();

            return bean;
        }

        public T Delete(int id)
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

        private bool Exists(long id)
        {
            return db.AsNoTracking().Any(e => e.Id == id);
        }
    }
}