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
        public int IdProduto { get; set; }
        public int IdSku { get; set; }
    }
}