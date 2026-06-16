using Microsoft.EntityFrameworkCore;
using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Part> Parts { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Part>(entity =>
            {
                entity.ToTable("parts");

                entity.Property(p => p.CategoryId)
                      .HasColumnName("category_id");

                entity.Property(p => p.ManufacturerId)
                      .HasColumnName("manufacturer_id");

                entity.Property(p => p.CreatedAt)
                      .HasColumnName("created_at");

                entity.Property(p => p.IsArchived)
                      .HasColumnName("is_archived");

                entity.Property(p => p.Price)
                      .HasPrecision(10, 2);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");

                entity.Property(e => e.FirstName)
                      .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                      .HasColumnName("last_name");

                entity.Property(e => e.IsAdmin)
                      .HasColumnName("is_admin");
            });
        }

    }
}
