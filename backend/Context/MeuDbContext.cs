using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Context
{
    public class MeuDbContext : DbContext
    {
        public MeuDbContext()
        {

            this.Database.EnsureCreated();
        }

            public MeuDbContext(DbContextOptions<MeuDbContext> options)
            : base(options)
        {

            this.Database.EnsureCreated();
        }
        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Cards> cards { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}



