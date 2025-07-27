using Perks.Perks;
using Perks.Perks.Kind;

namespace Perks
{
    /// <summary>
    /// Simulate how likely it is to get 3x Perk Wave Requirement within one run.
    /// </summary>
    public class PerkSimulator
    {
        const int RUNS = 100_000;
        const int PERKS_TO_DRAW = 4;
        const int SELECTIONS = 8;

        public void Simulate()
        {
            Dictionary<int, int> dict = new()
            {
                {0, 0 },
                {1, 0 },
                {2, 0 },
                {3, 0 }
            };

            for (int run = 0; run < RUNS; run++)
            {
                var availablePerks = new PerkSelector(
                    [.. Enumeration.GetAll<BasicPerk>().Select(p => new PerkStock(p, p.MaxQuantity))],
                    [.. Enumeration.GetAll<UWPerk>().Select(p => new PerkStock(p, p.MaxQuantity))],
                    [.. Enumeration.GetAll<TradeOffPerk>().Select(p => new PerkStock(p, p.MaxQuantity))]
                    );

                availablePerks.FirstPerk = BasicPerk.PerkWaveRequirement;

                // only have 4 UWs + 1 from the random UW perk unlocked => rest can be removed (assume i always get CL, does not affect result)
                availablePerks.BanPerk(UWPerk.ChronoField);
                availablePerks.BanPerk(UWPerk.SmartMissles);
                availablePerks.BanPerk(UWPerk.InnerLandmines);
                availablePerks.BanPerk(UWPerk.PoisonSwamp);

                availablePerks.BanPerk(BasicPerk.DefenseAbsolute);
                availablePerks.BanPerk(BasicPerk.Interest);
                availablePerks.BanPerk(BasicPerk.HealthRegen);
                //availablePerks.BanPerk(BasicPerk.LandMineDamage);
                //availablePerks.BanPerk(BasicPerk.BounceShot);

                int waveReuirementPerkCounter = 0;
                for (int i = 0; i < SELECTIONS; i++)
                {
                    var perkOffer = availablePerks.Draw(PERKS_TO_DRAW);
                    var perkToChose =
                        perkOffer.Draws.FirstOrDefault(po => po == BasicPerk.PerkWaveRequirement) ??
                        perkOffer.Draws.FirstOrDefault(po => po == BasicPerk.MaxGameSpeed) ??
                        perkOffer.Draws.FirstOrDefault(po => po == BasicPerk.RandomUW) ??
                        perkOffer.Draws.FirstOrDefault(po => po == BasicPerk.Orbs);
                    if (perkToChose == BasicPerk.PerkWaveRequirement)
                    {
                        waveReuirementPerkCounter++;
                    }
                    var perkSelection = perkOffer.Chose(perkToChose ?? perkOffer.Draws[0]);

                    availablePerks.Select(perkSelection);
                }

                dict[waveReuirementPerkCounter] += 1;
            }

            foreach (var item in dict)
            {
                Console.WriteLine($"{item.Key}: {(item.Value / (decimal)RUNS) * 100.0m,6:N2} % ");
            }
        }
    }
}
