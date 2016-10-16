using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class Sku: IAbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeReduzido { get; set; }
        public string Codigo { get; set; }
        public string Modelo { get; set; }
        public string Ean { get; set; }
        public string Url { get; set; }
        public bool ForaDeLinha { get; set; }
        public int Estoque { get; set; }
        public Dimensoes Dimensoes { get; set; }
        virtual public ICollection<Imagem> Imagens { get; set; }
        virtual public ICollection<Grupo> Grupos { get; set; }
        virtual public ICollection<SkuMarketplaceGetResponse> Marketplaces { get; set; }
        virtual public ICollection<Disponibilidade> Disponibilidades { get; set; }
        public string CodigoProduto { get; set; }

        // Initializes enumerable properties
        public Sku()
        {
            Imagens = new List<Imagem>();
            Grupos = new List<Grupo>();
            Marketplaces = new List<SkuMarketplaceGetResponse>();
            Disponibilidades = new List<Disponibilidade>();
        }
    }
}