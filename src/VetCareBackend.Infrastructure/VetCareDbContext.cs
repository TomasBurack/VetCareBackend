using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Domain.Entities;

namespace VetCareBackend.Infrastructure
{
    public class VetCareDbContext : DbContext
    {
        public DbSet<Sysadmin> Sysadmins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }

        public VetCareDbContext(DbContextOptions<VetCareDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().UseTpcMappingStrategy();
        }
    }
}
