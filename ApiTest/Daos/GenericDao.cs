using ApiTest.Daos;
using ApiTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ApiTest.Daos
{
    public class GenericDao<T>: AbstractDao<T> where T: class, IAbstractModel
    {
        public GenericDao() : base()
        {
        }
    }
}