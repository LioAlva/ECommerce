using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Index("Product_CompanyId_Description_Index", 1, IsUnique = true)]
        [Index("Product_CompanyId_BarCode_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [MaxLength(50, ErrorMessage = "the filed {0} must be maximo {1} characters lenght")]
        [Display(Name = "Product")]
        [Index("Product_CompanyId_Description_Index", 2, IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [MaxLength(13, ErrorMessage = "the filed {0} must be maximun {1} characters lenght")]
        [Display(Name = "Bar Code")]
        [Index("Product_CompanyId_BarCode_Index", 2, IsUnique = true)]
        public string BarCode { get; set; }


        [Required(ErrorMessage = "the field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "the field {0} is requiered")]
        [Range(1, double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Tax")]
        public int TaxId { get; set; }


        [Required(ErrorMessage = "The field {0} is requiered")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]//currency formato de modneda con 2 decimales
        [Range(0, double.MaxValue, ErrorMessage = "You must select  a {0} between {1} and {2}")]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [NotMapped]
        [Display(Name ="Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        //es solo lectura se puede colocar atributos 
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]//currency formato de modneda con 2 ,salchicas quezo luca
        public double Stock { get { return Inventories ==null?0: Inventories.Sum(i => i.Stock); }}

        public virtual Company Company { get; set; }

        public virtual  Category Category { get; set; }

        public virtual Tax Tax { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<OrderDetailTmp> OrderDetailTmps { get; set; }
        
    }
}