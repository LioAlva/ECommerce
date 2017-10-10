using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Index("City_Name_Index",2,IsUnique=true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [Range(1,double.MaxValue,ErrorMessage ="You must select a {0}")]
        [Index("City_Name_Index", 1, IsUnique = true)]
        [Display(Name="Department")]
        public int DepartmentId { get; set; }

        public virtual  Department Department { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Warehouse> Warehouses { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}