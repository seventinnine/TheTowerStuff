using UltimateWeapons.Helper;
using static UltimateWeapons.Helper.MathOperations;

namespace UltimateWeapons;

/// <summary>
/// Simulates the average coverage of black holes.
/// </summary>
public class BlackHoleCoverageSimulator
{
    private const int CIRCLE_DEGREES = 360;
    private const decimal stepSize = 0.01m;

    /// <summary>
    /// Calculates the percentage of the tower range diameter (from <paramref name="towerRange"/>) covered by <paramref name="numberOfBlackHoles"/> black holes.
    /// </summary>
    /// <param name="towerRange">Tower Range - radius of the main circle</param>
    /// <param name="blackHoleRadius">Black Hole Radius</param>
    /// <param name="numberOfBlackHoles">Number of black holes</param>
    /// <returns>Percentage of diameter covered by black holes (0.0-1.0)</returns>
    public decimal Simulate(decimal towerRange, decimal blackHoleRadius, int numberOfBlackHoles)
    {
        // Generate N random black hole positions
        var blackHoles = GenerateBlackHoles(towerRange * 0.9m, blackHoleRadius, numberOfBlackHoles);

        int coveredDiameterSteps = 0;

        int totalSteps = (int)(CIRCLE_DEGREES / stepSize);
        // Iterate over all 360 degrees of the diameter
        for (int i = 0; i < totalSteps; i++)
        {
            decimal angle = i * stepSize; // Current angle in degrees
            decimal angleInRadians = angle * (decimal)Math.PI / 180m; // Convert to radians

            // Calculate point on the tower diameter (at the edge of tower range)
            decimal x = towerRange * (decimal)Math.Cos((double)angleInRadians);
            decimal y = towerRange * (decimal)Math.Sin((double)angleInRadians);

            Point currentPoint = new Point(x, y);

            // Check if current point is within any black hole
            bool isStepWithinAnyBlackHole = false;
            foreach (var blackHole in blackHoles)
            {
                if (IsPointInCircle(currentPoint, blackHole.Center, blackHole.Radius))
                {
                    isStepWithinAnyBlackHole = true;
                    break;
                }
            }

            if (isStepWithinAnyBlackHole)
            {
                coveredDiameterSteps++;
            }
        }

        return (decimal)coveredDiameterSteps / totalSteps;
    }

    /// <summary>
    /// Generates N evenly spaced black hole positions on a circle of given radius
    /// </summary>
    private static List<BlackHole> GenerateBlackHoles(decimal innerRadius, decimal blackHoleRadius, int count)
    {
        var blackHoles = new List<BlackHole>();

        for (int i = 0; i < count; i++)
        {
            // Compute equally spaced angle for each black hole
            double angle = i * (2 * Math.PI / count);

            // Calculate position on the circle
            decimal x = innerRadius * (decimal)Math.Cos(angle);
            decimal y = innerRadius * (decimal)Math.Sin(angle);

            blackHoles.Add(new BlackHole(new Point(x, y), blackHoleRadius * 1.5m));
        }

        return blackHoles;
    }

    private struct BlackHole
    {
        public Point Center { get; set; }
        public decimal Radius { get; set; }

        public BlackHole(Point center, decimal radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}