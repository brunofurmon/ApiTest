using ApiTest.Daos;
using ApiTest.Models;
using System.Collections.Generic;


namespace ApiTest.Services
{
    public interface IAbstractService<T> where T: class, IAbstractModel
    {
        // List
        List<T> List();
        //Get id
        T Get(int id);
        //Create
        T Create(T bean);
        // Update
        T Update(T bean);
        //Delete
        T Delete(int id);
    }

    public abstract class AbstractService<T>: IAbstractService<T> where T: class, IAbstractModel
    {
        private IGenericDao<T> dao { get; set; }

        public AbstractService() : base()
        {
        }

        // List
        public List<T> List()
        {
            List<T> beans;
            using (GenericDao<T> dao = new GenericDao<T>())
            {
                beans = dao.List();
            }
                
            return beans;
        }

        //Get id
        public T Get(int id)
        {
            T bean;
            using (GenericDao<T> dao = new GenericDao<T>())
            {
                bean = dao.Get(id);
            }
            return bean;
        }
        //Create
        public T Create(T bean)
        {
            T success;
            using (GenericDao<T> dao = new GenericDao<T>())
            {
                success = dao.Create(bean);
            }
            return success;
        }
        // Update
        public T Update(T bean)
        {
            T success;
            using (GenericDao<T> dao = new GenericDao<T>())
            {
                success = dao.Update(bean);
            }
            return success;
        }
        //Delete
        public T Delete(int id)
        {
            T success;
            using (GenericDao<T> dao = new GenericDao<T>())
            {
                success = dao.Delete(id);
            }
            return success;
        }
    }
}