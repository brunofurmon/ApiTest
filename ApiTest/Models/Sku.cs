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

        public static Sku FromOrderForm(OrderForm form)
        {
            Sku createdSku = new Sku
            {
                IdProduto = form.Parametros.IdProduto,
                IdSku = form.Parametros.IdSku,
                Preco = form.Parametros.Preco,
                Disponivel = form.Parametros.Disponivel
            };

            return createdSku;
        }
    }
}