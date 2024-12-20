using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
   

    public class Towing_Collection : IdentityDbContext<Admin, IdentityRole<int>, int>
    {
        public Towing_Collection(DbContextOptions<Towing_Collection> options) : base(options)
        {
        }

        public DbSet<Admin> Admin_table { get; set; }
        public DbSet<Driver> Driver_table { get; set; }
        public DbSet<Customer> Customer_table { get; set; }
        public DbSet<Order> Order_table { get; set; }
        public DbSet<Service> Service_table { get; set; }
        public DbSet<Customer_driver> Customer_driver_table { get; set; }
        public DbSet<Customer_order> Customer_order_table { get; set; }
        public DbSet<Order_Driver> Order_Driver_table { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تكوين العلاقات بين الجداول الخاصة بك
            modelBuilder.Entity<Customer_driver>()
                .HasOne(cd => cd.Drivers)
                .WithMany(d => d.Customer_drivers)
                .HasForeignKey(cd => cd.DriverId);

            modelBuilder.Entity<Order_Driver>()
                .HasKey(od => new { od.DriverId, od.OrderId });

            modelBuilder.Entity<Order_Driver>()
                .HasOne(od => od.Drivers)
                .WithMany(d => d.Order_Drivers)
                .HasForeignKey(od => od.DriverId);

            modelBuilder.Entity<Order_Driver>()
                .HasOne(od => od.Orders)
                .WithMany(o => o.Order_Drivers)
                .HasForeignKey(od => od.OrderId);
        }
    }
}
