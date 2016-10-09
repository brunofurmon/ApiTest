using ApiTest.Dto;
using System.ComponentModel.DataAnnotations;
using static ApiTest.Components.ApiEnums;


namespace ApiTest.Models
{
    public class Sku: AbstractModel
    {
        [Required]
        public long IdProduto { get; set; }
        [Required]
        public long IdSku { get; set; }
        [Required]
        public decimal Preco { get; set; }
        public Availability Disponivel { get; set; }

        public static Sku FromForm(SkuForm form)
        {
            Sku createdSku = new Sku
            {
                IdProduto = form.IdProduto,
                IdSku = form.IdSku,
                Preco = form.Preco,
                Disponivel = form.Disponivel
            };

            return createdSku;
        }
    }
}