namespace UltimateWeapons.Helper;

public class MathOperations
{
    /// <summary>
    /// Checks if a point is inside a circle
    /// </summary>
    public static bool IsPointInCircle(Point point, Point center, decimal radius)
    {
        decimal dx = point.X - center.X;
        decimal dy = point.Y - center.Y;
        decimal distanceSquared = dx * dx + dy * dy;
        return distanceSquared <= radius * radius;
    }
}
