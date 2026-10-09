using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokedexAPI_New.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemon",
                table: "Pokemon");

            migrationBuilder.RenameTable(
                name: "Pokemon",
                newName: "Pokemons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "Id", "Description", "Height", "IsEncountered", "Name", "Type", "Weight" },
                values: new object[,]
                {
                    { 1, "Seed Pokémon", 0.69999999999999996, true, "Bulbasaur", "Grass/Poison", 6.9000000000000004 },
                    { 2, "Lizard Pokémon", 0.59999999999999998, true, "Charmander", "Fire", 8.5 },
                    { 3, "Tiny Turtle Pokémon", 0.5, true, "Squirtle", "Water", 9.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons");

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "Pokemons",
                newName: "Pokemon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemon",
                table: "Pokemon",
                column: "Id");
        }
    }
}
