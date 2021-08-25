//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace G3Pharmaceuticals.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class TypeProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TypeProduct()
        {
            this.Products = new HashSet<Product>();
        }
        [Display(Name = "Type ID")]
        [Required(ErrorMessage = "Please Enter Type ID")]
        [Remote("IsIDNameExist", "TypeProduct", ErrorMessage = "Type ID already exist")]
        public string typeID { get; set; }
        [Display(Name = "Type Name")]
        [Required(ErrorMessage = "Please Enter Type Name")]
        public string typeName { get; set; }
        [Display(Name = "Image")]
        public string images { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
