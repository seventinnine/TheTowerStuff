
namespace UltimateWeapons.Cycleables.Weapons;

public class GoldenTower : UltimateWeapon
{
    public override string Name => "Golden Tower";
    // Bonus - Duration - Cooldown
    public ScalingValue Lab_GTBonus { get; }
    public ScalingValue Lab_GTDuration { get; }

    public GoldenTower(UltimateWeaponProperties properties, ScalingValue lab_GTBonus, ScalingValue lab_GTDuration, bool perkEnabled) : base(properties, perkEnabled)
    {
        Lab_GTBonus = lab_GTBonus;
        Lab_GTDuration = lab_GTDuration;
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
        var totalMultiplier = Properties.Slot1.CurrentValue + Lab_GTBonus.CurrentValue;
        if (PerkEnabled)
        {
            totalMultiplier = (totalMultiplier - ModuleSubEffects.Slot1) * 1.5m + ModuleSubEffects.Slot1;
        }
        return totalMultiplier;
    }

    public static GoldenTower Create()
    {
        var gtBonus = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 5.0m, valuePerLevel: 0.8m) { Name = "Multiplier" };
        var gtDuration = new LinearScalingValue(level: 0, maxLevel: 38, startValue: 15, valuePerLevel: 1) { Name = "Duration" };
        var gtCooldown = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 300, valuePerLevel: -10) { Name = "Cooldown" };
        var gtWorkshop = new UltimateWeaponProperties(gtBonus, gtDuration, gtCooldown);
        var gtBonusLab = new LinearScalingValue(level: 0, maxLevel: 25, startValue: 0, valuePerLevel: 0.15m) { Name = "Bonus" };
        var gtDurationLab = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 0, valuePerLevel: 1) { Name = "Duration" };
        return new GoldenTower(gtWorkshop, gtBonusLab, gtDurationLab, false) { PerkDescription = "Golden Tower bonus x1.5" };
    }

    public override IReadOnlyList<ScalingValue> GetLabSlots()
    {
        return [Lab_GTBonus, Lab_GTDuration];
    }
}
