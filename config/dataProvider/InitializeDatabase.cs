
using luchito_net.Models;
using Microsoft.EntityFrameworkCore;

namespace luchito_net.Config.DataProvider
{
    public class InitializeDatabase(DbContextOptions options) : DbContext(options)
    {

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<State> State { get; set; }

        public DbSet<Provider> Provider { get; set; }

        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<State>().HasData(
                new State { Id = 1, Name = "Pendiente" },
                new State { Id = 2, Name = "Comprado" },
                new State { Id = 3, Name = "Distribuido" }
            );
        }


    }
}