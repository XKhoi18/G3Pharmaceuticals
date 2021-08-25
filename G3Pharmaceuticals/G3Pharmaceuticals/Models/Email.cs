using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace G3Pharmaceuticals.Models
{
    public class Email
    {
        public string Address { get; set; }
        public string Tittle { get; set; }
        public string Message { get; set; }
    }
}