using UltimateWeapons.Cycleables;

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


    public void Simulate(List<Cycleable> cycleables, params Action[] beforeSecondRuns)
    {
        var res1 = Simulate(cycleables);
        Console.WriteLine(res1);
        foreach (var beforeSecondRun in beforeSecondRuns)
        {
            beforeSecondRun.Invoke();
            var res2 = Simulate(cycleables);
            Console.WriteLine(res2);
            Console.WriteLine($"Increase: {((res2.CoinsPerSecond / res1.CoinsPerSecond) - 1.0m) * 100.0m,3:N2} %");
        }
    }

    /// <summary>
    /// Simulates one synchronized cycle between all provided <paramref name="ultimateWeapons"/>. Assumes steady income flow of 1 coin/sec.
    /// Steps over this cycle second-wise and accumulates the currently applying coin multiplier.
    /// </summary>
    public static CycleInfo Simulate(List<Cycleable> ultimateWeapons)
    {
        int second = 0;
        decimal accumulatedMultiplier = 0.0m;
        do
        {
            decimal multiplier = 1.0m;
            foreach (Cycleable ultimateWeapon in ultimateWeapons)
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

    private static bool CanActivateUWsSimultaneously(List<Cycleable> ultimateWeapons)
    {
        return ultimateWeapons.All(uw => uw.CanActivate());
    }
}
