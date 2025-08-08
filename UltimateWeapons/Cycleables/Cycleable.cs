namespace UltimateWeapons.Cycleables;


public abstract class Cycleable
{
    /// <summary>
    /// Statistics of something that occurs in a cycle..
    /// </summary>
    /// <param name="Cooldown">Time between object triggers.</param>
    /// <param name="Duration">Effective time the object is active.</param>
    /// <param name="CoinMultiplier">Total coin gain multiplier (base is already factored in there).</param>
    /// <param name="Effectiveness">Correction factor for objects that only apply to a certain area. Only affects the bonus coin multiplier</param>
    public record class Stats(int Cooldown, int Duration, decimal CoinMultiplier, decimal Effectiveness = 1.0m)
    {
        /// <summary>
        /// Coin multiplier without the base 1.0m multiplier.
        /// </summary>
        public decimal BonusCoinMultiplier => CoinMultiplier - 1.0m;
    }
    public abstract string Name { get; }

    private Stats? cachedStats;
    public Stats CurrentStats => GetStats();

    public Stats GetStats()
    {
        if (cachedStats == null)
        {
            cachedStats = EvaluateStats();
        }
        return cachedStats;
    }

    public int RemainingCooldown { get; private set; }

    public void Activate() => RemainingCooldown = CurrentStats.Cooldown;
    public bool IsActive() => CurrentStats.Cooldown - RemainingCooldown <= CurrentStats.Duration;

    public void ForwardTime(int delta = 1) => RemainingCooldown -= delta;
    public bool CanActivate() => RemainingCooldown == 0;

    public decimal EffectiveMultiplier()
    {
        return CurrentStats.BonusCoinMultiplier * CurrentStats.Effectiveness + 1.0m;
    }

    protected abstract Stats EvaluateStats();
    public void ReEvaluate()
    {
        cachedStats = EvaluateStats();
    }
}
