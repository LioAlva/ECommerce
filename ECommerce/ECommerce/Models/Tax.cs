using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [MaxLength(50, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [Display(Name = "Tax")]
        [Index("Tax_CompanyId_Description_Index", 2, IsUnique = true)]
        public string Description { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered")]
        [DisplayFormat(DataFormatString ="{0:P2}", ApplyFormatInEditMode = false)]
        [Range(0,1, ErrorMessage = "You must select  a {0} between {1} and {2}")]
        public double Rate { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Index("Tax_CompanyId_Description_Index", 1 , IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}