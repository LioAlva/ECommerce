using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class ECommerceContext : DbContext
    {   //base es superclase
        public ECommerceContext() : base("DefaultConnection")
        {

        }
        //Para no borrar en cascada
        //Model builder hereda del DataContext
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public DbSet<Department> Departments { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tax> Taxes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        //se hace manualmente , dbset es persistencia a bd y la collection se llamara inventory 
        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ECommerce.Models.ApiModels.Orderr> Orderss { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<OrderDetailTmp> OrderDetailTmps { get; set; }

        public DbSet<CompanyCustomer> CompanyCustomers { get; set; }

    }
}