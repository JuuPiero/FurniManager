using FurniManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurniManager.Data
{
    public class ApplicationDbContext : DbContext
    {

        private static ApplicationDbContext _instance;

        public static ApplicationDbContext Instance
        {
            get
            {
                if (_instance == null) _instance = new ApplicationDbContext();
                return _instance;
            }
        }



        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> Attributes { get; set; }
        public DbSet<ProductImage> Images { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }


        public DbSet<SaleOrder> SaleOrders { get; set; }
        public DbSet<SaleOrderDetail> SaleOrderDetails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=LAPTOP-46DSA8AK\\SQLEXPRESS;Database=NoiThatDB;Trusted_Connection=True;TrustServerCertificate=Yes");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductImage>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductAttribute>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Attributes)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.PurchaseOrder)
                .WithMany(po => po.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.Product)
                .WithMany()
                .HasForeignKey(pod => pod.ProductId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<SaleOrderDetail>()
               .HasOne(so => so.SaleOrder)
               .WithMany(so => so.SaleOrderDetails)
               .HasForeignKey(so => so.SaleOrderId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleOrderDetail>()
                .HasOne(so => so.Product)
                .WithMany()
                .HasForeignKey(so => so.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

        }


        public static bool CheckLogin(string email, string password)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users.Any(u => u.Email == email && u.Password == password);
            }
        }

        public void UpdateOrCreate<T>(T entity) where T : class
        {
            using var db = new ApplicationDbContext();

            var dbSet = db.Set<T>(); // Lấy DbSet của thực thể
            var existing = dbSet.Find(db.Entry(entity).Property("Id").CurrentValue);

            if (existing == null)
                dbSet.Add(entity);
            else
                db.Entry(existing).CurrentValues.SetValues(entity);

            db.SaveChanges();
        }
    }
}
