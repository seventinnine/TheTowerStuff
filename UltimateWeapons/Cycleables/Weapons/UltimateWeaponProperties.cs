namespace UltimateWeapons.Cycleables.Weapons;

public class UltimateWeaponProperties
{
    public UltimateWeaponProperties(ScalingValue? slot1, ScalingValue slot2, ScalingValue slot3)
    {
        if (slot1 is not null)
        {
            Slot1 = slot1;
            slot1.SupportsModuleSubstat = true;
        }
        Slot2 = slot2;
        slot2.SupportsModuleSubstat = true;
        Slot3 = slot3;
        slot3.SupportsModuleSubstat = true;
    }

    public ScalingValue? Slot1 { get; }
    public ScalingValue Slot2 { get; }
    public ScalingValue Slot3 { get; }
}
