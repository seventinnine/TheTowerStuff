namespace UltimateWeapons;

public class UltimateWeaponCycling
{
    /// <summary>
    /// Simulation result.
    /// </summary>
    /// <param name="Duration">Duration of the cycle.</param>
    /// <param name="CoinsPerSecond">Effective coins/sec during the cycle.</param>
    public record struct CycleInfo(int Duration, decimal CoinsPerSecond)
    {
        public override readonly string ToString()
        {
            return $"Cylce time: {Duration,3:N0} s{Environment.NewLine}Coins/s: {CoinsPerSecond,3:N2} s";
        }
    }

    public void Simulate()
    {
        var sim = new BlackHoleCoverageSimulator();
        decimal towerRange = 69.5m;
        List<UltimateWeapon> uws1 = [];
        uws1.Add(new UltimateWeapon(new UltimateWeaponStats(200, 35, 16.98m))); // GT
        uws1.Add(new UltimateWeapon(new UltimateWeaponStats(300, 10, 2.0m))); // DW
        uws1.Add(new UltimateWeapon(new UltimateWeaponStats(200, 30, 7.5m, sim.Simulate(towerRange, 46.0m, 1)))); // BH
        var res1 = Simulate(uws1);
        Console.WriteLine(res1);
        List<UltimateWeapon> uws2 = [];
        uws2.Add(new UltimateWeapon(new UltimateWeaponStats(200, 35, 16.98m))); // GT
        uws2.Add(new UltimateWeapon(new UltimateWeaponStats(300, 10, 2.0m))); // DW
        uws2.Add(new UltimateWeapon(new UltimateWeaponStats(200, 30, 7.5m, sim.Simulate(towerRange, 50.0m, 2)))); // BH
        //uws2.Add(new UltimateWeapon(new UltimateWeaponStats(120, 20, 2.0m, 0.02m))); // GB
        var res2 = Simulate(uws2);
        Console.WriteLine(res2);
        Console.WriteLine($"Increase: {((res2.CoinsPerSecond / res1.CoinsPerSecond) - 1.0m) * 100.0m,3:N2} %");
    }

    /// <summary>
    /// Simulates one synchronized cycle between all provided <paramref name="ultimateWeapons"/>. Assumes steady income flow of 1 coin/sec.
    /// Steps over this cycle second-wise and accumulates the currently applying coin multiplier.
    /// </summary>
    public static CycleInfo Simulate(List<UltimateWeapon> ultimateWeapons)
    {
        int second = 0;
        decimal accumulatedMultiplier = 0.0m;
        do
        {
            decimal multiplier = 1.0m;
            foreach (UltimateWeapon ultimateWeapon in ultimateWeapons)
            {
                if (ultimateWeapon.CanActivate())
                {
                    ultimateWeapon.Activate();
                }
                ultimateWeapon.ForwardTime();
                if (ultimateWeapon.IsActive())
                {
                    multiplier *= ultimateWeapon.EffectiveMultiplier();
                }
            }
            second++;
            accumulatedMultiplier += multiplier;
        } while (!CanActivateUWsSimultaneously(ultimateWeapons));

        decimal coinsPerSecond = accumulatedMultiplier / second;

        return new CycleInfo(second, coinsPerSecond);
    }

    private static bool CanActivateUWsSimultaneously(List<UltimateWeapon> ultimateWeapons)
    {
        return ultimateWeapons.All(uw => uw.CanActivate());
    }
}
