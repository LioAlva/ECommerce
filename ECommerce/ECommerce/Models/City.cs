using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [MaxLength(50, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [Display(Name = "City")]
        public string Name { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        public int DepartmentId { get; set; }

        public virtual  Department Department { get; set; }
    }
}