using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Index("Department_Name_Index",IsUnique =true)]//creamos iun indice e la tabla departamaento del CAMPO NAME Y ES INDICE UNICO
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Warehouse> Warehouses { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        
    }
}