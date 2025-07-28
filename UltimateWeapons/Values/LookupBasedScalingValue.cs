namespace UltimateWeapons;


public class LookupBasedScalingValue(int level, int maxLevel, Dictionary<int, decimal> lookup) : ScalingValue(level, maxLevel)
{
    public override decimal CurrentValue => lookup[Level];
}
