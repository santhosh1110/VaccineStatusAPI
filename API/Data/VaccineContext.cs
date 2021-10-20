using Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.Data
{
    public class VaccineContext : DbContext
    {

        public VaccineContext(DbContextOptions<VaccineContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Company>()
                .HasMany(x => x.Employees)
                .WithOne(x=>x.company)
                .OnDelete(DeleteBehavior.Cascade);


        }


        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

    }
}
