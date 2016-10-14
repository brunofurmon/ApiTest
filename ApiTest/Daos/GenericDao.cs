using ApiTest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;

namespace ApiTest.Daos
{
    public interface IGenericDao<T> where T : class, IAbstractModel
    {
        /// GET: api/T
        List<T> List();
        /// GET: api/T/5
        T Get(int bean);
        /// Search
        List<T> SearchFor(Expression<Func<T, bool>> predicate);
        /// PUT: api/Ts/5
        T Update(T bean);
        /// POST: api/Ts
        T Create(T bean);
        /// DELETE: api/Ts/5
        T Delete(int id);
    }

    public class GenericDao<T> : DbContext, IGenericDao<T> where T : class, IAbstractModel
    {
        public GenericDao() : base("name=ApiTest")
        {
            // Solves problem while trying to Serialize empty Disponibilidades List
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<T> db { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Maps inherited properties from models
            modelBuilder.Entity<T>().Map(m =>
            {
                m.MapInheritedProperties();
            });

            // One Sku to Many Disponibilidades
            modelBuilder.Entity<Disponibilidade>()
                    .HasRequired<Sku>(s => s.Sku)
                    .WithMany(s => s.Disponibilidades);

            // Does not try to pluralize portuguese names in english (was generating "Grupoes, Imagems, AtributoDoGrupoes" and stuff like that...)
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public virtual List<T> List()
        {
            if (db.Count() == 0)
            {
                return new List<T>();
            }
            return db.ToList();
        }

        public virtual T Get(int id)
        {
            T bean = db.Find(id);
            if (bean != null)
            {
                return bean;
            }
            return null;
        }
        /// <summary>
        /// /////////////////////
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> bean = db.Where(predicate);
            return (bean == null)? null: bean.ToList();
        }

        public virtual T Update(T bean)
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

        public virtual T Create(T bean)
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

        private bool Exists(int id)
        {
            return db.AsNoTracking().Any(b => b.Id == id);
        }
    }
}