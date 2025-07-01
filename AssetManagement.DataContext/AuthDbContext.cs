
using AssetManagement.DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AssetManagement.DataContext
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<PagePermission> PagePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; } // Add this DbSet

        public AuthDbContext(DbContextOptions<AuthDbContext> options, bool ensureCreated = true)
            : base(options)
        {
            if (ensureCreated)
                Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships and other mappings
            builder.Entity<PagePermission>()
                .HasOne(pp => pp.ApplicationRole)
                .WithMany(ar => ar.PagePermissions)
                .HasForeignKey(pp => pp.ApplicationRoleId)
                .IsRequired();

            // Configure UserPermission relationships if needed
            // Example:
            // builder.Entity<UserPermission>()
            //     .HasOne(up => up.User)
            //     .WithMany()
            //     .HasForeignKey(up => up.UserId)
            //     .IsRequired();
        }
    }
}
