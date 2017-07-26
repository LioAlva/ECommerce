using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage ="the field {0} is requiered")]
        [MaxLength(50,ErrorMessage ="the filed {0} must be maximo {1} characters lenght")]
        [Display(Name="Department")]
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}