using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class AtributoDoGrupo: IAbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }

        // Foreign Key
        [JsonIgnore]
        public int GrupoId { get; set; }
        [JsonIgnore]
        public virtual Grupo Grupo { get; set; }
    }
}