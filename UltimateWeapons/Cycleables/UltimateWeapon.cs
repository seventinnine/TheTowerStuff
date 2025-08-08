using UltimateWeapons.Cycleables.Weapons;

namespace UltimateWeapons.Cycleables;

public abstract class UltimateWeapon(UltimateWeaponProperties properties, bool perkEnabled) : Cycleable
{
    public ModuleBonuses ModuleSubEffects { get; set; } = new();
    // Damage - Effect Waves - Cooldown
    public UltimateWeaponProperties Properties { get; set; } = properties;
    public string PerkDescription { get; set; }
    public bool PerkEnabled { get; set; } = perkEnabled;

    public abstract IReadOnlyList<ScalingValue> GetLabSlots();
}
