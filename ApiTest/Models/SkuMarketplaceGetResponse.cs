﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public class SkuMarketplaceGetResponse: IAbstractModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string CodigoMarketplace { get; set; }
        public string CodigoCategoria { get; set; }

        // Foreign Key
        [JsonIgnore]
        public int SkuId { get; set; }
        [JsonIgnore]
        public virtual Sku Sku { get; set; }
    }
}