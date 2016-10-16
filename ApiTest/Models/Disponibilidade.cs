using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class Disponibilidade: IAbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public decimal Preco { get; set; }
        public decimal PrecoDe { get; set; }
        public bool Disponivel { get; set; }
        public string CodigoMarketplace { get; set; }

        // Foreign Key
        [JsonIgnore]
        public int SkuId { get; set; }
        [JsonIgnore]
        public virtual Sku Sku { get; set; }
    }
}