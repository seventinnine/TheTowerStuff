using Perks.Perks.Kind;

namespace Perks.Perks
{
    /// <summary>
    /// Perk the player has chosen + current perk level.
    /// </summary>
    /// <param name="perk"></param>
    /// <param name="level"></param>
    class ChosenPerk(Perk perk, int level)
    {
        public Perk Perk { get; } = perk;
        public int Level { get; set; } = level;
    }
}
