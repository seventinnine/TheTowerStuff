namespace UltimateWeapons
{
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


    public class UltimateWeaponCycling
    {
        public void Simulate()
        {
            var sim = new BlackHoleCoverageSimulator();
            decimal towerRange = 69.5m;
            List<UltimateWeapon> uws1 = [];
            uws1.Add(new UltimateWeapon(new UltimateWeaponStats(200, 35, 16.98m))); // GT
            uws1.Add(new UltimateWeapon(new UltimateWeaponStats(300, 10, 2.0m))); // DW
            uws1.Add(new UltimateWeapon(new UltimateWeaponStats(200, 30, 7.5m, sim.Calc(towerRange, 46.0m, 1)))); // BH
            var res1 = Simulate(uws1);
            Console.WriteLine(res1);
            List<UltimateWeapon> uws2 = [];
            uws2.Add(new UltimateWeapon(new UltimateWeaponStats(200, 35, 16.98m))); // GT
            uws2.Add(new UltimateWeapon(new UltimateWeaponStats(300, 10, 2.0m))); // DW
            uws2.Add(new UltimateWeapon(new UltimateWeaponStats(200, 30, 7.5m, sim.Calc(towerRange, 46.0m, 2)))); // BH
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
}
