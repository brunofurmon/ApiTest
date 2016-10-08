using ApiTest.Components;
using System;

namespace ApiTest.Dto
{
    public class OrderForm
    {
        public string Tipo { get; set; }
        public DateTime DataEnvio { get; set; }
        public Parametros Parametros { get; set; }
    }

    public class Parametros
    {
        public long IdProduto { get; set; }
        public long IdSku { get; set; }
        public decimal Preco { get; set; }
    }
}