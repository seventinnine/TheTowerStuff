namespace Perks.Perks.Kind
{
    record class BasicPerk(string Name, int MaxQuantity) : Perk(Name, MaxQuantity)
    {
        public static readonly BasicPerk MaxHealth = new("x1.20 Max Health", 5);
        public static readonly BasicPerk Damage = new("x1.15 Damage", 5);
        public static readonly BasicPerk CoinBonus = new("x1.15 All Coin Bonuses", 5);
        public static readonly BasicPerk DefenseAbsolute = new("x1.15 Defense Absolute", 5);
        public static readonly BasicPerk CashBonus = new("x1.15 Cash Bonus", 5);
        public static readonly BasicPerk HealthRegen = new("x1.75 Health Regen", 5);
        public static readonly BasicPerk Interest = new("Interest x1.50", 5);
        public static readonly BasicPerk LandMineDamage = new("Land Mine Damage x3.50", 5);
        public static readonly BasicPerk FreeUpgradeChance = new("Free Upgrade Chance for All +5.0%", 5);
        public static readonly BasicPerk DefensePercent = new("Defense Percent +4.00", 5);
        public static readonly BasicPerk BounceShot = new("Bounce Shot +2", 3);
        public static readonly BasicPerk PerkWaveRequirement = new("Perk Wave Requirement -20.00%", 3);
        public static readonly BasicPerk Orbs = new("Orbs +1", 2);
        public static readonly BasicPerk RandomUW = new("Unlock a Random Ultimate Weapon", 1);
        public static readonly BasicPerk MaxGameSpeed = new("Increase Max Game Speed by +1.00", 1);
    }
}
