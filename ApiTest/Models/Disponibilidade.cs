using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class Disponibilidade
    {
        public int DisponibilidadeId { get; set; }
        public decimal Preco { get; set; }
        public decimal PrecoDe { get; set; }
        public bool Disponivel { get; set; }
        public string CodigoMarketplace { get; set; }

        // Foreign Key
        public int SkuId { get; set; }
        public virtual Sku Sku { get; set; }
    }
}