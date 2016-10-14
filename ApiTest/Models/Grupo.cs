using Apiest.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    [ComplexType]
    public class Grupo
    {
        public string Nome { get; set; }
        virtual public ICollection<AtributoDoGrupo> Atributos { get; set; }

        public Grupo()
        {
            Atributos = new List<AtributoDoGrupo>();
        }
    }
}