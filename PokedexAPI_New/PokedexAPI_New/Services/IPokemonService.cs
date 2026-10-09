using PokedexAPI_New.Models;

namespace PokedexAPI_New.Services
{
    public interface IPokemonService
    {
        IEnumerable<Pokemon> GetAll();
        Pokemon? GetById(int id);
        IEnumerable<Pokemon> Search(string name);
        Pokemon Add(Pokemon pokemon);
        Pokemon? Update(int id, Pokemon updated);
        bool Delete(int id);
    }
}