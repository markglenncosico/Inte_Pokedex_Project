using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokedexAPI_New.Migrations
{
    /// <inheritdoc />
    public partial class AddStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "Atk",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Def",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Hp",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpAtk",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpDef",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Spd",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpriteUrl",
                table: "Pokemons",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atk",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Def",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Hp",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "SpAtk",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "SpDef",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "Spd",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "SpriteUrl",
                table: "Pokemons");

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
    }
}
