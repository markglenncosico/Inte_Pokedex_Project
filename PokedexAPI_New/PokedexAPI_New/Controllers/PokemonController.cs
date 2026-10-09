using Microsoft.AspNetCore.Mvc;
using PokedexAPI_New.Data;
using PokedexAPI_New.Models;

namespace PokedexAPI_New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly PokedexContext _context;

        public PokemonController(PokedexContext context)
        {
            _context = context;
        }

        // GET: api/pokemon
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Pokemons.ToList());
        }

        // GET: api/pokemon/25
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var pokemon = _context.Pokemons.Find(id);
            if (pokemon == null) return NotFound($"Pokemon with ID {id} not found.");
            return Ok(pokemon);
        }

        // GET: api/pokemon/search?name=char
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Search name cannot be empty.");

            var results = _context.Pokemons
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToList();

            return Ok(results);
        }

        // POST: api/pokemon
        [HttpPost]
        public IActionResult Add(Pokemon newPokemon)
        {
            if (string.IsNullOrWhiteSpace(newPokemon.Name))
                return BadRequest("Name is required.");

            try
            {
                _context.Pokemons.Add(newPokemon);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = newPokemon.Id }, newPokemon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/pokemon/25
        [HttpPut("{id}")]
        public IActionResult Update(int id, Pokemon updated)
        {
            if (string.IsNullOrWhiteSpace(updated.Name))
                return BadRequest("Name is required.");

            try
            {
                var pokemon = _context.Pokemons.Find(id);
                if (pokemon == null) return NotFound($"Pokemon with ID {id} not found.");

                pokemon.Name          = updated.Name;
                pokemon.Type          = updated.Type;
                pokemon.Height        = updated.Height;
                pokemon.Weight        = updated.Weight;
                pokemon.Description   = updated.Description;
                pokemon.IsEncountered = updated.IsEncountered;
                pokemon.SpriteUrl     = updated.SpriteUrl;
                pokemon.Hp            = updated.Hp;
                pokemon.Atk           = updated.Atk;
                pokemon.Def           = updated.Def;
                pokemon.SpAtk         = updated.SpAtk;
                pokemon.SpDef         = updated.SpDef;
                pokemon.Spd           = updated.Spd;

                _context.SaveChanges();
                return Ok(pokemon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/pokemon/25
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var pokemon = _context.Pokemons.Find(id);
                if (pokemon == null) return NotFound($"Pokemon with ID {id} not found.");

                _context.Pokemons.Remove(pokemon);
                _context.SaveChanges();
                return Ok(new { message = $"{pokemon.Name} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}