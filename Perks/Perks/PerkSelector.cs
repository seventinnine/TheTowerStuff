using Perks.Perks.Kind;

namespace Perks.Perks
{
    /// <summary>
    /// Manages everything around the set of perks currently applying in a run.
    /// </summary>
    /// <param name="basicPerks">All the basic perks that exist + their available stock</param>
    /// <param name="uwPerks">All the UW perks that exist + their available stock</param>
    /// <param name="tradeOffPerks">All the Trade Offer perks that exist + their available stock</param>
    class PerkSelector(List<PerkStock> basicPerks, List<PerkStock> uwPerks, List<PerkStock> tradeOffPerks)
    {
        /// <summary>
        /// Perk the player selects from the <see cref="PerkOffer"/>
        /// </summary>
        /// <param name="ChosenPerk"></param>
        /// <param name="RejectedPerks"></param>
        public record class PerkSelection(Perk ChosenPerk, List<Perk> RejectedPerks);

        /// <summary>
        /// Perks the player gets offered to pick from.
        /// </summary>
        /// <param name="draws">Perks the player can pick from</param>
        public class PerkOffer(List<Perk> draws)
        {
            public IReadOnlyList<Perk> Draws { get; } = draws;

            /// <summary>
            /// Choses <paramref name="perk"/>, rejects the rest.
            /// </summary>
            /// <param name="perk"></param>
            /// <returns></returns>
            public PerkSelection Chose(Perk perk)
            {
                return new PerkSelection(perk, draws.Where(d => d != perk).ToList());
            }
        }

        private static readonly Random rnd = new();
        private readonly List<ChosenPerk> chosenPerks = [];

        /// <summary>
        /// Already chosen perks + their levels.
        /// </summary>
        public IReadOnlyList<ChosenPerk> ChosenPerks => chosenPerks;


        /// <summary>
        /// Perk that is guaranteed on the first draw.
        /// </summary>
        public BasicPerk? FirstPerk { get; set; }

        /// <summary>
        /// Make this perk no longer appear in the selection.
        /// </summary>
        /// <param name="perk">Perk to yeet</param>
        public void BanPerk(Perk perk)
        {
            BagForPerk(perk).FirstOrDefault(p => p.Perk == perk)?.Deplete();
        }

        /// <summary>
        /// Draw from a set of perks.
        /// </summary>
        /// <param name="count">How many perks the player can select</param>
        /// <returns></returns>
        public PerkOffer Draw(int count)
        {
            List<Perk> draws = [];

            for (int i = 0; i < count; i++)
            {
                PerkStock draw;
                if (FirstPerk is not null)
                {
                    draw = BagForPerk(FirstPerk).First(p => p.Perk == FirstPerk);
                    FirstPerk = null;
                }
                else
                {
                    List<PerkStock> bag = RollBag();
                    draw = bag.Where(p => p.CanTake() && !draws.Any(d => d == p.Perk)).ToList().Draw(rnd);
                }

                draw.Take();
                draws.Add(draw.Perk);
            }

            return new PerkOffer(draws);
        }

        /// <summary>
        /// Randomly draws from all available bags (based on availability).
        /// </summary>
        /// <returns>Returns a random draw-</returns>
        /// <exception cref="Exception"></exception>
        private List<PerkStock> RollBag()
        {
            List<(int, List<PerkStock>)> owo = [];
            if (tradeOffPerks.Where(b => b.CanTake()).Count() > 0)
            {
                owo.Add((15, tradeOffPerks));
            }
            if (uwPerks.Where(b => b.CanTake()).Count() > 0)
            {
                owo.Add((20, uwPerks));
            }
            if (basicPerks.Where(b => b.CanTake()).Count() > 0)
            {
                owo.Add((65, basicPerks));
            }
            int totalWeight = owo.Sum(b => b.Item1);
            int roll = rnd.Next(0, totalWeight + 1);

            int cumsum = 0;
            foreach (var item in owo)
            {
                cumsum += item.Item1;
                if (roll <= cumsum)
                {
                    return item.Item2;
                }
            }

            throw new Exception();
        }

        /// <summary>
        /// Finalizes the selected perk, puts all unselected perks back into their bags.
        /// </summary>
        /// <param name="perkSelection"></param>
        public void Select(PerkSelection perkSelection)
        {
            var match = chosenPerks.FirstOrDefault(cp => cp.Perk == perkSelection.ChosenPerk);
            if (match is null)
            {
                chosenPerks.Add(new ChosenPerk(perkSelection.ChosenPerk, 1));
            }
            else
            {
                match.Level += 1;
            }

            foreach (var rejectedPerk in perkSelection.RejectedPerks)
            {
                List<PerkStock> bag = BagForPerk(rejectedPerk);
                bag.FirstOrDefault(p => p.Perk == rejectedPerk)?.PutBack();
            }
        }

        /// <summary>
        /// Select the bag <param name="perk"></param> appears in.
        /// </summary>
        /// <param name="perk"></param>
        /// <returns></returns>
        private List<PerkStock> BagForPerk(Perk perk)
        {
            return perk switch
            {
                BasicPerk _ => basicPerks,
                UWPerk _ => uwPerks,
                _ => tradeOffPerks
            };
        }
    }
}
