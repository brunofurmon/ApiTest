using Apiest.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    [ComplexType]
    public class Grupo
    {
        public string Nome { get; set; }
        public AtributoDoGrupo[] Atributos { get; set; }
    }
}