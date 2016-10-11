using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    [ComplexType]
    public class Imagem
    {
        public string Menor { get; set; }
        public string Maior { get; set; }
        public string Zoom { get; set; }
        public int Ordem { get; set; }
    }
}