using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UnderPayroll.Web.Models;

namespace UnderPayroll.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("departments");

            entity.HasKey(d => d.Id);

            entity.Property(d => d.Id)
                .HasColumnName("id");

            entity.Property(d => d.Code)
                .HasColumnName("code")
                .HasMaxLength(10);

            entity.Property(d => d.Name)
                .HasColumnName("name")
                .HasMaxLength(60);

            entity.Property(d => d.Description)
                .HasColumnName("description");

            entity.Property(d => d.Location)
                .HasColumnName("location")
                .HasMaxLength(80);

            entity.Property(d => d.Budget)
                .HasColumnName("budget")
                .HasPrecision(14, 2);

            entity.Property(d => d.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);

            entity.Property(d => d.Email)
                .HasColumnName("email")
                .HasMaxLength(120);

            entity.Property(d => d.IsActive)
                .HasColumnName("is_active");

            entity.Property(d => d.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(d => d.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Document)
                .HasColumnName("document")
                .HasMaxLength(15);
            
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(60);
            
            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(60);
            
            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(120);
            
            entity.Property(e => e.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);
            
            entity.Property(e => e.Position)
                .HasColumnName("position")
                .HasMaxLength(80);
            
            entity.Property(e => e.Salary)
                .HasColumnName("salary")
                .HasPrecision(12, 2);

            entity.Property(e => e.HireDate)
                .HasColumnName("hire_date");
            
            entity.Property(e => e.DepartmentId)
                .HasColumnName("department_id");

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            entity.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}