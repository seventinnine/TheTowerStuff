namespace UltimateWeapons.Cycleables.Weapons;

public class BlackHole : UltimateWeapon
{
    public TowerStats TowerStats { get; }

    // Size - Duration - Cooldown
    public UltimateWeaponProperties Properties { get; }
    public ScalingValue Lab_BHExtra { get; }
    public ScalingValue Lab_BHCoinBonus { get; }
    public bool PerkEnabled { get; }

    public BlackHole(TowerStats towerStats, UltimateWeaponProperties properties, ScalingValue lab_BHExtra, ScalingValue lab_BHCoinBonus, bool perkEnabled) : base()
    {
        TowerStats = towerStats;
        Properties = properties;
        Lab_BHExtra = lab_BHExtra;
        Lab_BHCoinBonus = lab_BHCoinBonus;
        PerkEnabled = perkEnabled;
    }

    protected override Stats EvaluateStats()
    {
        return new Stats(EvaluateBHCooldown(), EvaluateBHDuration(), EvaluateBHBonus(), EvaluateBHCoverage());
    }

    private int EvaluateBHCooldown()
    {
        return (int)(Properties.Slot3.CurrentValue + ModuleSubEffects.Slot3);
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
}
