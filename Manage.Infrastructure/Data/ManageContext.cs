using Manage.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Infrastructure.Data
{
   public class ManageContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ManageContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<EmployeePersonalDetails> EmployeePersonalDetails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }

        //the name of the table will be same as the name of the class
        private static void SetTableNamesAsSingle(ModelBuilder builder)
        {
            // Use the entity name instead of the Context.DbSet<T> name
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                builder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            SetTableNamesAsSingle(builder);
            base.OnModelCreating(builder);

            //change the delete behaviour from cascade to restrict 
            foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //builder.Entity<ApplicationUser>()
            //    .Property(e => e.Id)
            //    .ValueGeneratedOnAdd();

            builder.Entity<ApplicationUser>()
                .HasOne<EmployeePersonalDetails>(p => p.EmployeePersonalDetails)
                .WithOne(s => s.ApplicationUser)
                .HasForeignKey<EmployeePersonalDetails>(e => e.Id);

            builder.Entity<EmployeeLeave>()
                .HasKey(el => new { el.EmployeeId, el.LeaveId });

            builder.Entity<EmployeeLeave>()
                .HasOne(el => el.Employee)
                .WithMany(l => l.EmployeeLeaves)
                .HasForeignKey(el => el.EmployeeId);

            builder.Entity<EmployeeLeave>()
                .HasOne(el => el.Leave)
                .WithMany(e => e.EmployeeLeaves)
                .HasForeignKey(el => el.LeaveId);
        }

    }
}
