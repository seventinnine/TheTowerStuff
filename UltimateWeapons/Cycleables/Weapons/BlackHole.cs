

namespace UltimateWeapons.Cycleables.Weapons;

public class BlackHole : UltimateWeapon
{
    public TowerStats TowerStats { get; set; }
    public override string Name => "Black Hole";

    // Size - Duration - Cooldown
    public ScalingValue Lab_BHExtra { get; }
    public ScalingValue Lab_BHCoinBonus { get; }

    public BlackHole(TowerStats towerStats, UltimateWeaponProperties properties, ScalingValue lab_BHExtra, ScalingValue lab_BHCoinBonus, bool perkEnabled) : base(properties, perkEnabled)
    {
        TowerStats = towerStats;
        Lab_BHExtra = lab_BHExtra;
        Lab_BHCoinBonus = lab_BHCoinBonus;
    }

    protected override Stats EvaluateStats()
    {
        return new Stats(EvaluateBHCooldown(), EvaluateBHDuration(), EvaluateBHBonus(), EvaluateBHCoverage());
    }

    private int EvaluateBHCooldown()
    {
        return (int)(Properties.Slot3.CurrentValue - ModuleSubEffects.Slot3);
    }

    private int EvaluateBHDuration()
    {
        return (int)(Properties.Slot2.CurrentValue + (PerkEnabled ? 12 : 0) + ModuleSubEffects.Slot2);
    }

    private decimal EvaluateBHBonus()
    {
        return Lab_BHCoinBonus.CurrentValue;
    }

    private decimal EvaluateBHCoverage()
    {
        var sim = new BlackHoleCoverageSimulator();

        return sim.Simulate(TowerStats.TowerRadius, EvaluateBHRadius(), (int)(1 + Lab_BHExtra.CurrentValue));
    }

    private decimal EvaluateBHRadius()
    {
        return (int)(Properties.Slot1.CurrentValue + ModuleSubEffects.Slot1);
    }

    public static BlackHole Create(TowerStats towerStats)
    {
        var bhSize = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 30.0m, valuePerLevel: 2.0m) { Name = "Size" };
        var bhDuration = new LinearScalingValue(level: 0, maxLevel: 23, startValue: 15, valuePerLevel: 1) { Name = "Duration" };
        var bhCooldown = new LinearScalingValue(level: 0, maxLevel: 15, startValue: 200, valuePerLevel: -10) { Name = "Cooldown" };
        var bhWorkshop = new UltimateWeaponProperties(bhSize, bhDuration, bhCooldown);
        var bhCoinBonusLab = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 1.0m, valuePerLevel: 0.5m) { Name = "Coin Bonus" };
        var bhExtraLab = new LinearScalingValue(level: 0, maxLevel: 2, startValue: 0, valuePerLevel: 1) { Name = "Extra Black Hole" };
        return new BlackHole(towerStats, bhWorkshop, bhExtraLab, bhCoinBonusLab, false) { PerkDescription = "Black Hole duration +12.0s" };
    }

    public override IReadOnlyList<ScalingValue> GetLabSlots()
    {
        return [Lab_BHCoinBonus, Lab_BHExtra];
    }
}
