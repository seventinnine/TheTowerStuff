namespace UltimateWeapons;

public abstract class ScalingValue(int level, int maxLevel)
{
    public string Name { get; set; }
    public int Level { get; set; } = level;
    public int MaxLevel { get; set; } = maxLevel;

    public abstract decimal CurrentValue { get; }
    public bool SupportsModuleSubstat { get; set; } = false;
    public decimal AddedBonus { get; set; }
}