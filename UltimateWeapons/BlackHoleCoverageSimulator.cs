using UltimateWeapons.Helper;

namespace UltimateWeapons;

/// <summary>
/// Simulates the average coverage of black holes.
/// </summary>
public class BlackHoleCoverageSimulator
{
    private const int CIRCLE_DEGREES = 360;
    private const decimal stepSize = 1.0m;

    /// <summary>
    /// Calculates the percentage of the tower range diameter (from <paramref name="towerRange"/>) covered by <paramref name="numberOfBlackHoles"/> black holes.
    /// </summary>
    /// <param name="towerRange">Tower Range - radius of the main circle</param>
    /// <param name="blackHoleRadius">Black Hole Radius</param>
    /// <param name="numberOfBlackHoles">Number of black holes</param>
    /// <returns>Percentage of diameter covered by black holes (0.0-1.0)</returns>
    public decimal Simulate(decimal towerRange, decimal blackHoleRadius, int numberOfBlackHoles)
    {
        //// Generate N random black hole positions
        //var blackHoles = GenerateBlackHoles(towerRange * 0.9m, blackHoleRadius, numberOfBlackHoles);

        //int coveredDiameterSteps = 0;

        //int totalSteps = (int)(CIRCLE_DEGREES / stepSize);
        //// Iterate over all 360 degrees of the diameter
        //for (int i = 0; i < totalSteps; i++)
        //{
        //    decimal angle = i * stepSize; // Current angle in degrees
        //    decimal angleInRadians = angle * (decimal)Math.PI / 180m; // Convert to radians

        //    // Calculate point on the tower diameter (at the edge of tower range)
        //    decimal x = towerRange * (decimal)Math.Cos((double)angleInRadians);
        //    decimal y = towerRange * (decimal)Math.Sin((double)angleInRadians);

        //    Point currentPoint = new Point(x, y);

        //    // Check if current point is within any black hole
        //    bool isStepWithinAnyBlackHole = false;
        //    foreach (var blackHole in blackHoles)
        //    {
        //        if (IsPointInCircle(currentPoint, blackHole.Center, blackHole.Radius))
        //        {
        //            isStepWithinAnyBlackHole = true;
        //            break;
        //        }
        //    }

        //    if (isStepWithinAnyBlackHole)
        //    {
        //        coveredDiameterSteps++;
        //    }
        //}
        //var res = (decimal)coveredDiameterSteps / totalSteps;

        var res = Math.Min(1.0m, CalculateDiameterCoveragePercentage(towerRange, blackHoleRadius, 0.9m) * numberOfBlackHoles);

        return res;
    }

    /// <summary>
    /// Calculates what percentage of the outer circle's diameter is covered by the inner circle
    /// </summary>
    /// <param name="outerCircleRadius">Radius of the outer circle</param>
    /// <param name="innerCircleRadius">Radius of the inner circle</param>
    /// <param name="innerCirclePositionPercent">Position of inner circle center as percentage of outer radius [0.0 ... 1.0]</param>
    /// <returns>Percentage of outer circle diameter covered by inner circle [0.0 ... 1.0]</returns>
    public static decimal CalculateDiameterCoveragePercentage(
        decimal outerCircleRadius,
        decimal innerCircleRadius,
        decimal innerCirclePositionPercent)
    {
        // Validate inputs
        if (outerCircleRadius <= 0)
            throw new ArgumentException("Outer circle radius must be positive", nameof(outerCircleRadius));

        if (innerCircleRadius <= 0)
            throw new ArgumentException("Inner circle radius must be positive", nameof(innerCircleRadius));

        if (innerCirclePositionPercent < 0.0m || innerCirclePositionPercent > 1.0m)
            throw new ArgumentException("Inner circle position must be between 0.0 and 1.0", nameof(innerCirclePositionPercent));

        // Convert position percentage to actual distance from center
        decimal distanceFromCenter = outerCircleRadius + innerCirclePositionPercent;

        // Place outer circle at origin, inner circle at (distanceFromCenter, 0)
        // Calculate intersection of inner circle with the main diameter (horizontal line y=0)
        decimal coverageLength = CalculateHorizontalDiameterCoverage(
            outerCircleRadius,
            innerCircleRadius,
            distanceFromCenter);

        // Calculate percentage of diameter covered
        decimal outerDiameter = 2 * outerCircleRadius;
        decimal coveragePercentage = coverageLength / outerDiameter;

        return Math.Min(coveragePercentage, 1.0m); // Cap at 100%
    }

    /// <summary>
    /// Calculates how much of the horizontal diameter (from -outerRadius to +outerRadius) 
    /// is covered by an inner circle positioned at (offsetX, 0)
    /// </summary>
    private static decimal CalculateHorizontalDiameterCoverage(
        decimal outerRadius,
        decimal innerRadius,
        decimal offsetX)
    {
        // Inner circle equation: (x - offsetX)² + y² = innerRadius²
        // Diameter line equation: y = 0
        // Substituting: (x - offsetX)² = innerRadius²
        // So: x = offsetX ± innerRadius

        decimal leftEdge = offsetX - innerRadius;
        decimal rightEdge = offsetX + innerRadius;

        // The diameter spans from -outerRadius to +outerRadius
        decimal diameterLeft = -outerRadius;
        decimal diameterRight = outerRadius;

        // Find the intersection of the inner circle coverage with the diameter
        decimal coverageStart = Math.Max(leftEdge, diameterLeft);
        decimal coverageEnd = Math.Min(rightEdge, diameterRight);

        // If there's no overlap, return 0
        if (coverageStart >= coverageEnd)
            return 0m;

        return coverageEnd - coverageStart;
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