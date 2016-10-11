using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    [ComplexType]
    public class Dimensoes
    {
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Peso { get; set; }
    }
}