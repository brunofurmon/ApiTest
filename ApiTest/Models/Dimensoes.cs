using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class Dimensoes: IAbstractModel
    {
        // One-to-zero-or-one
        [Key, ForeignKey("Sku")]
        [JsonIgnore]
        public int Id { get; set; }
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Peso { get; set; }

        // Foreign Key
        [JsonIgnore]
        public virtual Sku Sku { get; set; }
    }
}