﻿using Apiest.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class Grupo: IAbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        virtual public ICollection<AtributoDoGrupo> Atributos { get; set; }

        public Grupo()
        {
            Atributos = new List<AtributoDoGrupo>();
        }
    }
}