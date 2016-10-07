using ApiTest.Models;
using ApiTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ApiTest.Services
{
    public class SkuService : AbstractService<Sku>, IAbstractService<Sku>
    {
        public SkuService() : base()
        {
        }
    }
}