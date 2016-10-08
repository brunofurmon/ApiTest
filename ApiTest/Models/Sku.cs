using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ApiTest.Models
{
    public class Sku: AbstractModel
    {
        [Required]
        public long ProductId { get; set; }
        [Required]
        public long SkuId { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}