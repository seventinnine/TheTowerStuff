namespace UltimateWeapons;

/// <summary>
/// Instance of an ultimate weapon.
/// </summary>
/// <param name="stats"></param>
public class UltimateWeapon(UltimateWeaponStats stats)
{
    public int RemainingCooldown { get; private set; }

    public void Activate() => RemainingCooldown = stats.Cooldown;
    public bool IsActive() => (stats.Cooldown - RemainingCooldown) <= stats.Duration;

    public void ForwardTime(int delta = 1) => RemainingCooldown -= delta;
    public bool CanActivate() => RemainingCooldown == 0;

    public decimal EffectiveMultiplier()
    {
        return stats.BonusCointMultiplier * stats.Effectiveness + 1.0m;
    }

}
