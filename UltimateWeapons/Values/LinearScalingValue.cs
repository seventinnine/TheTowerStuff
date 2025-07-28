namespace UltimateWeapons;

public class LinearScalingValue(int level, int maxLevel, decimal startValue, decimal valuePerLevel) : ScalingValue(level, maxLevel)
{
    public decimal StartValue { get; } = startValue;
    public decimal ValuePerLevel { get; } = valuePerLevel;

    public override decimal CurrentValue => StartValue + Level * ValuePerLevel;
}
