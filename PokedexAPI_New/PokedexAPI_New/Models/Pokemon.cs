namespace PokedexAPI_New.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public double Height { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; } = "";
        public bool IsEncountered { get; set; } = true;
        public string? SpriteUrl { get; set; }

        // Base stats
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int SpAtk { get; set; }
        public int SpDef { get; set; }
        public int Spd { get; set; }
    }
}