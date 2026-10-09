using System.Net.Http;
using System.Text.Json;
using PokedexAPI_New.Data;
using PokedexAPI_New.Models;

namespace PokedexAPI_New.Services
{
    public class PokeApiImporter
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task ImportPokemons(PokedexContext context, int count = 151)
        {
            // ✅ If ANY pokemon exists, skip seeding entirely
            if (context.Pokemons.Any()) return;

            for (int id = 1; id <= count; id++)
            {
                var response = await client.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{id}");
                using var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                var name   = root.GetProperty("name").GetString() ?? "Unknown";
                var height = root.GetProperty("height").GetInt32() / 10.0;
                var weight = root.GetProperty("weight").GetInt32() / 10.0;

                var types = root.GetProperty("types")
                    .EnumerateArray()
                    .Select(t => t.GetProperty("type").GetProperty("name").GetString())
                    .Where(t => t != null)
                    .ToArray();

                // ✅ Read base stats
                var statsArr = root.GetProperty("stats").EnumerateArray().ToList();
                int hp    = statsArr[0].GetProperty("base_stat").GetInt32();
                int atk   = statsArr[1].GetProperty("base_stat").GetInt32();
                int def   = statsArr[2].GetProperty("base_stat").GetInt32();
                int spAtk = statsArr[3].GetProperty("base_stat").GetInt32();
                int spDef = statsArr[4].GetProperty("base_stat").GetInt32();
                int spd   = statsArr[5].GetProperty("base_stat").GetInt32();

                // ✅ Read sprite URL
                var spriteUrl = root
                    .GetProperty("sprites")
                    .GetProperty("other")
                    .GetProperty("official-artwork")
                    .GetProperty("front_default")
                    .GetString();

                // Fetch flavor text
                var speciesResponse = await client.GetStringAsync($"https://pokeapi.co/api/v2/pokemon-species/{id}");
                using var speciesDoc = JsonDocument.Parse(speciesResponse);

                var flavorTextEntry = speciesDoc.RootElement
                    .GetProperty("flavor_text_entries")
                    .EnumerateArray()
                    .FirstOrDefault(e => e.GetProperty("language").GetProperty("name").GetString() == "en");

                var flavorText = flavorTextEntry.ValueKind != JsonValueKind.Undefined
                    ? flavorTextEntry.GetProperty("flavor_text").GetString() ?? "No description available"
                    : "No description available";

                flavorText = flavorText.Replace("\n", " ").Replace("\f", " ");

                var pokemon = new Pokemon
                {
                    Id            = id,
                    Name          = char.ToUpper(name[0]) + name.Substring(1),
                    Type          = string.Join("/", types),
                    Height        = height,
                    Weight        = weight,
                    Description   = flavorText,
                    IsEncountered = true,
                    SpriteUrl     = spriteUrl,
                    Hp    = hp,
                    Atk   = atk,
                    Def   = def,
                    SpAtk = spAtk,
                    SpDef = spDef,
                    Spd   = spd
                };

                context.Pokemons.Add(pokemon);
                await context.SaveChangesAsync();
                Console.WriteLine($"Added {pokemon.Name}");
            }
        }
    }
}