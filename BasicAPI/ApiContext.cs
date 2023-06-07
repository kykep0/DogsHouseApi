using BasicAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace BasicAPI
{
    public class ApiContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>().HasData(
                new Dog { Id = 1, Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
                new Dog { Id = 2, Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 }
            );
        }
    }
}
