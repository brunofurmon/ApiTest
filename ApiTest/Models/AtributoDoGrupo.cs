using System.ComponentModel.DataAnnotations.Schema;


namespace Apiest.Models
{
    [ComplexType]
    public class AtributoDoGrupo
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}