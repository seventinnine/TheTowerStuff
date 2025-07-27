namespace Perks.Perks.Kind
{
    record class TradeOffPerk(string Name) : Perk(Name, 1)
    {
        public static readonly TradeOffPerk TowerDamage_BossHealth = new("x1.50 Tower Damage, but Bosses Have 8x Health");
        public static readonly TradeOffPerk Coins_TowerHealth = new("x1.80 coins, but Tower Max Health -70%");
        public static readonly TradeOffPerk EnemyHealth_TowerRegenLifesteal = new("Enemies Have -50% Health, but Tower Health Regen and Lifesteal -90%");
        public static readonly TradeOffPerk EnemyDamage_TowerDamage = new("Enemies Damage -50%, but Tower Damage -50%");
        public static readonly TradeOffPerk RangedRange_RangedDamage = new("Ranged Enemies Attack Distance Reduced, But Tower Ranged Enemies Damage x3");
        public static readonly TradeOffPerk EnemySpeed_EnemyDamage = new("Enemies Speed -40%, But Enemies Damage x2.5");
        public static readonly TradeOffPerk CashPerWave_CashPerKill = new("x12.00 Cash Per Wave, But Enemy Kill Don't Give Cash");
        public static readonly TradeOffPerk TowerRegen_TowerHealth = new("Tower Health Regen x8.00, But Tower Max Max Health -60%");
        public static readonly TradeOffPerk BossHealth_BossSpeed = new("Boss Health -70%, But Boss Speed +50%");
        public static readonly TradeOffPerk Lifesteal_Knockback = new("Lifesteal x2.50, But Knockback force -70%");
    }
}
