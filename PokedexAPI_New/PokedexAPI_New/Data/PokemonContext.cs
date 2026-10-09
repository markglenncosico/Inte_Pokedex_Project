using Microsoft.EntityFrameworkCore;
using PokedexAPI_New.Models;

namespace PokedexAPI_New.Data
{
    public class PokedexContext : DbContext
    {
        public PokedexContext(DbContextOptions<PokedexContext> options) : base(options) { }

        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<User> Users { get; set; }
    }
}