using ApiTest.Models;
using System.Data.Entity;


namespace ApiTest.Daos
{
    public class SkuDao : AbstractDao<Sku>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SkuDao() : base()
        {
        }
    }
}
