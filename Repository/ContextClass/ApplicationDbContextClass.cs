using Domain.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ContextClass
{
    public  class ApplicationDbContextClass : DbContext
    {
        public ApplicationDbContextClass(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerItems> CustomerItems { get; set; }
        public DbSet<SupplierItems> SuppliersItems { get; set; }
        public DbSet<ItemImages> ItemImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne<UserType>(u => u.UserType)
                .WithMany(u => u.User)
                .HasForeignKey(u => u.UserTypeId)
                .IsRequired();

            modelBuilder.Entity<CustomerItems>()
                .HasOne<User>(u => u.User)
                .WithMany(u => u.CustomerItems)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            modelBuilder.Entity<SupplierItems>()
                .HasOne<User>(u => u.User)
                .WithMany(u => u.SupplierItems)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            modelBuilder.Entity<Items>()
                .HasOne<Category>(c => c.Category)
                .WithMany(i => i.Items)
                .HasForeignKey(f => f.Category_Id)
                .IsRequired();

            modelBuilder.Entity<ItemImages>()
                 .HasOne<Items>(i => i.Items)
                 .WithMany(i => i.ItemImages)
                 .HasForeignKey(i => i.ItemId)
                 .IsRequired();

            modelBuilder.Entity<CustomerItems>()
                .HasOne<Items>(i => i.Items)
                .WithOne(u => u.CustomerItems)
                .HasForeignKey<CustomerItems>(u => u.ItemId)
                .IsRequired();

            modelBuilder.Entity<SupplierItems>()
                .HasOne<Items>(i => i.Items)
                .WithOne(i => i.SuppliersItems)
                .HasForeignKey<SupplierItems>(s => s.ItemId)
                .IsRequired();            
        }

    }
}
