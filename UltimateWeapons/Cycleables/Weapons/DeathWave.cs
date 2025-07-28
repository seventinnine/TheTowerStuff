namespace UltimateWeapons.Cycleables.Weapons;


public class DeathWave : UltimateWeapon
{
    // Damage - Effect Waves - Cooldown
    public UltimateWeaponProperties Properties { get; }
    public ScalingValue Lab_DWCoinBonus { get; }
    public bool PerkEnabled { get; }

    public DeathWave(UltimateWeaponProperties properties, ScalingValue lab_DWCoinBonus, bool perkEnabled) : base()
    {
        Properties = properties;
        Lab_DWCoinBonus = lab_DWCoinBonus;
        PerkEnabled = perkEnabled;
    }

    protected override Stats EvaluateStats()
    {
        return new Stats(EvaluateDWCooldown(), EvaluateDWDuration(), EvaluateDWBonus());
    }

    private int EvaluateDWCooldown()
    {
        return (int)(Properties.Slot3.CurrentValue + ModuleSubEffects.Slot3);
    }

    private int EvaluateDWDuration()
    {
        return (int)(Properties.Slot2.CurrentValue + (PerkEnabled ? 1 : 0) + ModuleSubEffects.Slot2) * 5;
    }

    private decimal EvaluateDWBonus()
    {
        return Lab_DWCoinBonus.CurrentValue;
    }
}
