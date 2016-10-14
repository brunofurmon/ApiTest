using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class SkuMarketplaceGetResponse: IAbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CodigoMarketplace { get; set; }
        public string CodigoCategoria { get; set; }
    }
}