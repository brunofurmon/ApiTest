using ApiTest.Dto;

namespace ApiTest.Models
{
    public class Sku : AbstractModel
    {
        public string Nome { get; set; }
        public string NomeReduzido { get; set; }
        public string Codigo { get; set; }
        public string Modelo { get; set; }
        public string Ean { get; set; }
        public string Url { get; set; }
        public bool ForaDeLinha { get; set; }
        public int Estoque { get; set; }
        public Dimensoes Dimensoes { get; set; }
        public Imagem[] Imagens { get; set; }
        public Grupo[] Grupos { get; set; }
        public SkuMarketplaceGetResponse[] Marketplaces { get; set; }
        public string CodigoProduto { get; set; }
    }
}