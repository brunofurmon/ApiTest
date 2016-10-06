using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EpicomTest.Models
{
    public class Sku
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int SkuId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
    }
}