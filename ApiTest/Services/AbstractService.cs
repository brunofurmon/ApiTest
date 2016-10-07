using ApiTest.Daos;
using ApiTest.Models;
using System.Collections.Generic;


namespace ApiTest.Services
{
    public interface IAbstractService<T>
    {
        // List
        List<T> List();
        //Get id
        T Get(long id);
        //Create
        T Create(T bean);
        // Update
        T Update(long id, T bean);
        //Delete
        T Delete(long id);
    }

    public class AbstractService<T>: IAbstractService<T> where T: AbstractModel
    {
        private IAbstractDao<T> dao { get; set; }

        public AbstractService() : base()
        {
            this.dao = new AbstractDao<T>();
        }

        // List
        public List<T> List()
        {
            List<T> beans = dao.List();
            return beans;
        }
        //Get id
        public T Get(long id)
        {
            T bean = dao.Get(id);
            return bean;
        }
        //Create
        public T Create(T bean)
        {
            T success = dao.Create(bean);
            return success;
        }
        // Update
        public T Update(long id, T bean)
        {
            T success = dao.Update(id, bean);
            return success;
        }
        //Delete
        public T Delete(long id)
        {
            T success = dao.Delete(id);
            return success;
        }
    }
}