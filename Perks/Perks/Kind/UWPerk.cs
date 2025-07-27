namespace Perks.Perks.Kind
{
    record class UWPerk(string Name) : Perk(Name, 1)
    {
        public static readonly UWPerk SmartMissles = new("4 More Smart Missles");
        public static readonly UWPerk PoisonSwamp = new("Swamp Radius x1.5");
        public static readonly UWPerk DeathWave = new("+1 Wave on Death Wave");
        public static readonly UWPerk InnerLandmines = new("Extra Set of Inner Mines");
        public static readonly UWPerk GoldenTower = new("Golden Tower Bonus x1.5");
        public static readonly UWPerk ChainLightning = new("Chain Lightning Damage x2");
        public static readonly UWPerk ChronoField = new("Chrono Field Duration +5s");
        public static readonly UWPerk BlackHole = new("Black Hole Duration +12.0s");
        public static readonly UWPerk Spotlight = new("Spotlight Damage Bonus x1.5");

    }
}
