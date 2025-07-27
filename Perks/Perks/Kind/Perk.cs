namespace Perks.Perks.Kind
{
    /// <summary>
    /// Base class for Perk.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="MaxQuantity"></param>
    abstract record class Perk(string Name, int MaxQuantity) : Enumeration(Name);
}
