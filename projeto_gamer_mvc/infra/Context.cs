using Microsoft.EntityFrameworkCore;
using projeto_gamer_mvc.Models;

namespace projeto_gamer_mvc.infra
{
    public class Context : DbContext
    {
        public Context()
        {

        }
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source = NOTE01-S14; initial catalog = gamerTarde; User ID=sa; pwd=Senai@134; TrustServerCertificate = true");
            }
        }

        public DbSet<Jogador> Jogador { get; set; }
        public DbSet<Equipe> Equipe { get; set; }
    }
}