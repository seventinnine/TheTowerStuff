using System.Reflection;

namespace Perks
{
    /// <summary>
    /// Base for enumeration classes.
    /// </summary>
    public abstract record class Enumeration : IComparable
    {
        public string Name { get; private set; }

        protected Enumeration(string name) => (Name) = (name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public int CompareTo(object? other) => Name.CompareTo(((Enumeration?)other)?.Name);

        public override int GetHashCode() => Name.GetHashCode();
    }
}
