using UltimateWeapons;
using UltimateWeapons.Cycleables;
using UltimateWeapons.Cycleables.Bots;
using UltimateWeapons.Cycleables.Weapons;

namespace Perks
{

    internal class Program
    {
        public static void Main(string[] args)
        {
            //var owo = new PerkSimulator();
            var owo = new UltimateWeaponCycling();

            var tower = new TowerStats(towerRadius: 69.5m);

            var gtBonus = new LinearScalingValue(level: 6, maxLevel: 50, startValue: 5.0m, valuePerLevel: 0.8m);
            var gtDuration = new LinearScalingValue(level: 4, maxLevel: 50, startValue: 15, valuePerLevel: 1);
            var gtCooldown = new LinearScalingValue(level: 10, maxLevel: 20, startValue: 300, valuePerLevel: -10);
            var gtWorkshop = new UltimateWeaponProperties(gtBonus, gtDuration, gtCooldown);
            var gtBonusLab = new LinearScalingValue(level: 11, maxLevel: 25, startValue: 0, valuePerLevel: 0.15m);
            var gtDurationLab = new LinearScalingValue(level: 14, maxLevel: 20, startValue: 0, valuePerLevel: 1);
            var gt = new GoldenTower(gtWorkshop, gtBonusLab, gtDurationLab, true);

            gt.ModuleSubEffects.Slot1 = 1.0m;

            var bhSize = new LinearScalingValue(level: 6, maxLevel: 50, startValue: 30.0m, valuePerLevel: 2.0m);
            var bhDuration = new LinearScalingValue(level: 4, maxLevel: 50, startValue: 15, valuePerLevel: 1);
            var bhCooldown = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 200, valuePerLevel: -10);
            var bhWorkshop = new UltimateWeaponProperties(bhSize, bhDuration, bhCooldown);
            var bhCoinBonusLab = new LinearScalingValue(level: 14, maxLevel: 20, startValue: 1.0m, valuePerLevel: 0.5m);
            var bhExtraLab = new LinearScalingValue(level: 1, maxLevel: 1, startValue: 0, valuePerLevel: 1);
            var bh = new BlackHole(tower, bhWorkshop, bhExtraLab, bhCoinBonusLab, true);

            bh.ModuleSubEffects.Slot2 = 2;
            bh.ModuleSubEffects.Slot1 = 8.0m;

            var dwDamage = new LookupBasedScalingValue(level: 1, maxLevel: 50, new Dictionary<int, decimal> { { 1, 2 } });
            var dwEffectWaves = new LinearScalingValue(level: 0, maxLevel: 5, startValue: 1, valuePerLevel: 1);
            var dwCooldown = new LinearScalingValue(level: 0, maxLevel: 20, startValue: 300, valuePerLevel: -10);
            var dwWorkshop = new UltimateWeaponProperties(dwDamage, dwEffectWaves, dwCooldown);
            var dwCoinBonusLab = new LinearScalingValue(level: 10, maxLevel: 20, startValue: 1.5m, valuePerLevel: 0.05m);
            var dw = new DeathWave(dwWorkshop, dwCoinBonusLab, true);

            var gbDuration = new LinearScalingValue(level: 0, maxLevel: 50, startValue: 20.0m, valuePerLevel: 0.5m);
            var gbCooldown = new LinearScalingValue(level: 0, maxLevel: 50, startValue: 120, valuePerLevel: -3);
            var gbBonus = new LinearScalingValue(level: 0, maxLevel: 50, startValue: 2.0m, valuePerLevel: 0.2m);
            var gbSize = new LinearScalingValue(level: 0, maxLevel: 50, startValue: 20.0m + 1.0m /* relic */, valuePerLevel: 2.0m);
            var gbEvents = new BotProperties(gbDuration, gbCooldown, gbBonus, gbSize);
            var gb = new GoldenBot(gbEvents);

            List<Cycleable> cycleables = [gt, bh, dw, gb];
            bh.ReEvaluate();
            owo.Simulate(cycleables,
                () =>
                {
                    Console.WriteLine(bh.GetStats().Effectiveness);
                    bhSize.Level += 1;
                    bh.ReEvaluate();
                    Console.WriteLine(bh.GetStats().Effectiveness);
                },
                () =>
                {
                    bhSize.Level -= 1;
                    bhDuration.Level += 1;
                    bh.ReEvaluate();
                },
                () =>
                {
                    bhDuration.Level -= 1;
                    bh.ReEvaluate();
                    gtBonus.Level += 1;
                    gt.ReEvaluate();
                },
                () =>
                {
                    gtBonus.Level -= 1;
                    gtDuration.Level += 1;
                    gt.ReEvaluate();
                });
        }
    }
}
