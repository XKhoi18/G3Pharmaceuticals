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

    public partial class Nation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nation()
        {
            this.Manufacturers = new HashSet<Manufacturer>();
        }
        [Display(Name = "Nation ID")]
        [Required(ErrorMessage = "Please Enter Nation ID")]
        [Remote("IsIDNameExist", "Nation", ErrorMessage = "Nation ID already exist")]
        public string nationID { get; set; }
        [Display(Name = "Nation Name")]
        [Required(ErrorMessage = "Please Enter Nation Name")]
        public string nationName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Manufacturer> Manufacturers { get; set; }
    }
}