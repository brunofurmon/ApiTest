using ApiTest.Components;
using System;
using static ApiTest.Components.ApiEnums;

namespace ApiTest.Dto
{
    public class OrderForm
    {
        public string Tipo { get; set; }
        public DateTime DataEnvio { get; set; }
        public SkuForm Parametros { get; set; }
    }

    public class SkuForm
    {
        public long IdProduto { get; set; }
        public long IdSku { get; set; }
        public decimal Preco { get; set; }
        public Availability Disponivel { get; set; } 
    }
}