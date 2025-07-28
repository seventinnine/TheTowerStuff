namespace UltimateWeapons.Cycleables;

public abstract class UltimateWeapon : Cycleable
{
    public ModuleBonuses ModuleSubEffects { get; set; } = new();
}
