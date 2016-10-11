using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    [ComplexType]
    public class SkuMarketplaceGetResponse
    {
        public string CodigoMarketplace { get; set; }
        public string CodigoCategoria { get; set; }
    }
}