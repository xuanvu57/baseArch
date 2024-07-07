using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RdbWithEF
{
    public class SampleEfDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// It will be call every time an instance of the context is created
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>();
        }
    }
}
