using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace G3Pharmaceuticals.Models
{
    public class CartItem
    {
        public Product product { get; set; }
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
    }
}