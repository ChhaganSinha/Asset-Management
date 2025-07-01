using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetManagement.Dto;
using AssetManagement.Dto.Models;

namespace AssetManagement.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, bool ensureCreated = true) : base(options) 
        {
            if (ensureCreated)
                Database.EnsureCreated();
        }
        public DbSet<UserProfilePicUpld> UserProfilePicUpld { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<ZoneArea> ZoneArea { get; set; }
        public DbSet<SubOffice> SubOffice { get; set; }
        public DbSet<Zoffice> Zoffice { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeFilesMapping> EmployeeFilesMapping { get; set; }
        public DbSet<Asset> Asset { get; set; }
        public DbSet<AssetType> AssetType { get; set; }
        public DbSet<SubAsset> SubAsset { get; set; }
        public DbSet<Allocation> Allocation { get; set; }
        public DbSet<AllocationLog> AllocationLog { get; set; }
        public DbSet<EmployeeSkills> EmployeeSkills { get; set; }
        public DbSet<DesignationDTO> Designation { get; set; }
        public DbSet<EmployeeSkillMapping> EmployeeSkillMapping { get; set; }
        public DbSet<EmployeeOnboardingDto> EmployeeOnboarding { get; set; }

        public DbSet<EmployeeInsurance> EmployeeInsurance { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>().Ignore(o => o.EmployeeId);
            modelBuilder.Entity<Asset>().Ignore(o => o.EmployeeName);
            modelBuilder.Entity<Asset>().Ignore(o => o.EmployeeEmail);
            modelBuilder.Entity<EmployeeOnboardingDto>().Ignore(o => o.ManagerMobile);
            base.OnModelCreating(modelBuilder);
        }

    }
}
