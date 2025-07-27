namespace UltimateWeapons;

/// <summary>
/// UW statistics.
/// </summary>
/// <param name="Cooldown">Time between UW triggers.</param>
/// <param name="Duration">Effective time the UW is active.</param>
/// <param name="CoinMultiplier">Total coin gain multiplier (base is already factored in there).</param>
/// <param name="Effectiveness">Correction factor for UWs that only apply to a certain area. Only affects the bonus coin multiplier</param>
public record class UltimateWeaponStats(int Cooldown, int Duration, decimal CoinMultiplier, decimal Effectiveness = 1.0m)
{
    /// <summary>
    /// Coin multiplier without the base 1.0m multiplier.
    /// </summary>
    public decimal BonusCointMultiplier => CoinMultiplier - 1.0m;
}
