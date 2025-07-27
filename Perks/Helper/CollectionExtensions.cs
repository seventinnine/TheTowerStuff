public static class CollectionExtensions
{
    public static T Draw<T>(this IList<T> items, Random random)
    {
        try
        {
            var n = random.Next(0, items.Count - 1);
            return items[n];

        }
        catch (Exception)
        {
            return default;
        }
    }
}
