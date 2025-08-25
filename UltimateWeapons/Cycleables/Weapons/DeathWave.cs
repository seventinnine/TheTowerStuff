
namespace UltimateWeapons.Cycleables.Weapons;


public class DeathWave : UltimateWeapon
{
    public override string Name => "Death Wave";

    public ScalingValue Lab_DWCoinBonus { get; }

    public DeathWave(UltimateWeaponProperties properties, ScalingValue lab_DWCoinBonus, bool perkEnabled) : base(properties, perkEnabled)
    {
        Lab_DWCoinBonus = lab_DWCoinBonus;
    }

    protected override Stats EvaluateStats()
    {
        return new Stats(EvaluateDWCooldown(), EvaluateDWDuration(), EvaluateDWBonus());
    }

    private int EvaluateDWCooldown()
    {
        return (int)(Properties.Slot3.CurrentValue - ModuleSubEffects.Slot3);
    }

    private int EvaluateDWDuration()
    {
        return (int)(Properties.Slot2.CurrentValue + (PerkEnabled ? 1 : 0) + ModuleSubEffects.Slot2) * 4 + 5;
    }

    private decimal EvaluateDWBonus()
    {
        return Lab_DWCoinBonus.CurrentValue;
    }

    public static DeathWave Create(/*Dictionary<int, decimal> deathWaveDamageLookup*/)
    {
        //var dwDamage = new LookupBasedScalingValue(level: 0, maxLevel: 30, deathWaveDamageLookup) { Name = "Damage" };
        var dwEffectWaves = new LinearScalingValue(level: 0, maxLevel: 4, startValue: 1, valuePerLevel: 1) { Name = "Effect Waves" };
        var dwCooldown = new LinearScalingValue(level: 0, maxLevel: 25, startValue: 300, valuePerLevel: -10) { Name = "Cooldown" };
        var dwWorkshop = new UltimateWeaponProperties(null, dwEffectWaves, dwCooldown);
        var dwCoinBonusLab = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 1.5m, valuePerLevel: 0.05m) { Name = "Coin Bonus" };
        return new DeathWave(dwWorkshop, dwCoinBonusLab, false) { PerkDescription = "+1 wave on Death Wave" };
    }

    public override IReadOnlyList<ScalingValue> GetLabSlots()
    {
        return [Lab_DWCoinBonus];
    }
}
