using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Inventory
    {
        //modelo mas sencillo pues no se ara con interfaz de usuario se hara con codigo
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public virtual Warehouse Warehouse { get; set; }

        public virtual Product  Product { get; set; }
    }
}