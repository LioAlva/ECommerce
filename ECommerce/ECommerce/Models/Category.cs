using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [MaxLength(50, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [Display(Name = "Category")]
        [Index("Category_Company_Name_Index", 2, IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Index("Category_Company_Name_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}