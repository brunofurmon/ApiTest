using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class Disponibilidade: AbstractModel
    {
        public decimal Preco { get; set; }
        public decimal PrecoDe { get; set; }
        public bool Disponivel { get; set; }
        public string CodigoMarketplace { get; set; }

            public int skuId { get; set; }
        [ForeignKey("SkuId")]
        public virtual Sku Sku { get; set; }
    }
}