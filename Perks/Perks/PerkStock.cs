using Perks.Perks.Kind;

namespace Perks.Perks
{
    /// <summary>
    /// Availability of a perk.
    /// </summary>
    /// <param name="perk"></param>
    /// <param name="remainingStock"></param>
    class PerkStock(Perk perk, int remainingStock)
    {
        public Perk Perk { get; } = perk;

        public bool CanTake()
        {
            return remainingStock > 0;
        }

        public void PutBack()
        {
            remainingStock++;
        }

        public void Take()
        {
            remainingStock--;
        }

        public void Deplete()
        {
            remainingStock = 0;
        }
    }
}
