namespace UltimateWeapons;

public abstract class ScalingValue(int level, int maxLevel)
{
    public int Level { get; set; } = level;
    public int MaxLevel { get; set; } = maxLevel;

    public abstract decimal CurrentValue { get; }
}