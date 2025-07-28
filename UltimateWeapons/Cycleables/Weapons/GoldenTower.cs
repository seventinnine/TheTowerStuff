namespace UltimateWeapons.Cycleables.Weapons;

public class GoldenTower : UltimateWeapon
{
    // Bonus - Duration - Cooldown
    public UltimateWeaponProperties Properties { get; }
    public ScalingValue Lab_GTBonus { get; }
    public ScalingValue Lab_GTDuration { get; }
    public bool PerkEnabled { get; }

    public GoldenTower(UltimateWeaponProperties properties, ScalingValue lab_GTBonus, ScalingValue lab_GTDuration, bool perkEnabled) : base()
    {
        Properties = properties;
        Lab_GTBonus = lab_GTBonus;
        Lab_GTDuration = lab_GTDuration;
        PerkEnabled = perkEnabled;
    }

    protected override Stats EvaluateStats()
    {
        return new Stats(EvaluateGTCooldown(), EvaluateGTDuration(), EvaluateGTBonus());
    }

    private int EvaluateGTCooldown()
    {
        return (int)(Properties.Slot3.CurrentValue + ModuleSubEffects.Slot3);
    }

    private int EvaluateGTDuration()
    {
        return (int)(Properties.Slot2.CurrentValue + Lab_GTDuration.CurrentValue + ModuleSubEffects.Slot2);
    }

    private decimal EvaluateGTBonus()
    {
        var totalMultiplier = Properties.Slot1.CurrentValue + Lab_GTBonus.CurrentValue + ModuleSubEffects.Slot1;
        if (PerkEnabled)
        {
            totalMultiplier = (totalMultiplier - 1.0m) * 1.5m + 1.0m;
        }
        return totalMultiplier;
    }
}
