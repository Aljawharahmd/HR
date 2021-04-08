using HR.Data.Enums;
using HR.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HR.Persistance
{
    public class HrDbContext : DbContext // ORM 
    {
        public HrDbContext(DbContextOptions<HrDbContext> options) : base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<LeaveCategory> LeavesCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Name).IsRequired();

                entity.HasMany(d => d.Employees)
                .WithOne(d => d.Department)
                .HasForeignKey(d => d.DepartmentId)
                .IsRequired(false);

                entity.HasOne(d => d.Manager)
                .WithOne(d => d.ManagedDepartment)
                .HasForeignKey<Department>(d => d.ManagerId);
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn(100, 1);

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.PhoneNumber).HasMaxLength(14);

                entity.HasOne(e => e.Department)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.DepartmentId).IsRequired(false);

                entity.HasMany(e => e.Leaves)
                .WithOne(e => e.Employee)
                .HasForeignKey(e => e.EmployeeId)
                .IsRequired(false);
            });

            modelBuilder.Entity<EmployeeLeave>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.HasOne(l => l.LeaveCategory)
                .WithMany(l => l.Leaves)
                .HasForeignKey(l => l.CategoryId);

                entity.HasOne(l => l.Employee)
                .WithMany(l => l.Leaves)
                .HasForeignKey(l => l.EmployeeId);

                entity.Property(l => l.CategoryId);

                entity.Property(l => l.StartDate).HasColumnType("date").IsRequired();
                entity.Property(l => l.EndDate).HasColumnType("date").IsRequired();

                entity.Property(l => l.Duration).IsRequired();
                entity.Property(l => l.Status).HasDefaultValue(LeaveStatus.Pending);
            });

            modelBuilder.Entity<LeaveCategory>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasMany(c => c.Leaves)
                .WithOne(c => c.LeaveCategory)
                .HasForeignKey(c => c.CategoryId);

                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.MaxDuration).IsRequired();
            });
        }
    }
}
