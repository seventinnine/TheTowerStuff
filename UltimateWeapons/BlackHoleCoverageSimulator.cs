namespace UltimateWeapons
{
    /// <summary>
    /// Simulates the average coverage of black holes.
    /// </summary>
    public class BlackHoleCoverageSimulator
    {
        public struct Point
        {
            public decimal X { get; set; }
            public decimal Y { get; set; }

            public Point(decimal x, decimal y)
            {
                X = x;
                Y = y;
            }
        }

        public struct BlackHole
        {
            public Point Center { get; set; }
            public decimal Radius { get; set; }

            public BlackHole(Point center, decimal radius)
            {
                Center = center;
                Radius = radius;
            }
        }
        private const int THREADS = 50;
        private const int TRIALS = 200;
        private static readonly Lock locker = new();
        private const int CIRCLE_DEGREES = 360;
        private const decimal stepSize = 0.1m;

        /// <summary>
        /// Calculates the percentage of the tower range diameter covered by N random black holes
        /// </summary>
        /// <param name="towerRange">Tower Range (TR) - radius of the main circle</param>
        /// <param name="innerTowerRange">Inner Tower Range (TRI) - radius where black holes spawn</param>
        /// <param name="blackHoleRadius">Black Hole Radius (BHR)</param>
        /// <param name="numberOfBlackHoles">Number of black holes (N)</param>
        /// <param name="seed">Random seed for reproducible results (optional)</param>
        /// <returns>Percentage of diameter covered by black holes (0-100)</returns>
        public decimal Calc(
            decimal towerRange,
            decimal blackHoleRadius,
            int numberOfBlackHoles)
        {
            decimal sumCoverage = 0.0m;
            Parallel.For(
                0, THREADS,
                () => 0.0m,
                (_, state, localAverage) =>
                {
                    for (int i = 0; i < TRIALS; i++)
                    {
                        localAverage += CalculateAverageDiamaterCoverage(towerRange, blackHoleRadius, numberOfBlackHoles);
                    }
                    return localAverage;
                },
                (localAverage) =>
                {
                    lock (locker)
                    {
                        sumCoverage += localAverage;
                    }
                });

            return sumCoverage / (THREADS * TRIALS);
        }

        private static decimal CalculateAverageDiamaterCoverage(decimal towerRange, decimal blackHoleRadius, int numberOfBlackHoles)
        {
            // Generate N random black hole positions
            var blackHoles = GenerateBlackHoles(towerRange * 0.9m, blackHoleRadius, numberOfBlackHoles);

            int coveredCounter = 0;

            int steps = (int)(CIRCLE_DEGREES / stepSize);
            // Iterate over all 360 degrees of the diameter
            for (int i = 0; i < steps; i++)
            {
                decimal angle = i * stepSize; // Current angle in degrees
                decimal angleInRadians = angle * (decimal)Math.PI / 180m; // Convert to radians

                // Calculate point on the tower diameter (at the edge of tower range)
                decimal x = towerRange * (decimal)Math.Cos((double)angleInRadians);
                decimal y = towerRange * (decimal)Math.Sin((double)angleInRadians);

                Point currentPoint = new Point(x, y);

                // Check if current point is within any black hole
                bool isWithinAnyBlackHole = false;
                foreach (var blackHole in blackHoles)
                {
                    if (IsPointInCircle(currentPoint, blackHole.Center, blackHole.Radius))
                    {
                        isWithinAnyBlackHole = true;
                        break;
                    }
                }

                if (isWithinAnyBlackHole)
                {
                    coveredCounter++;
                }
            }

            return (decimal)coveredCounter / steps;
        }

        /// <summary>
        /// Generates N random black hole positions on a circle of given radius
        /// </summary>
        private static List<BlackHole> GenerateBlackHoles(decimal innerRadius, decimal blackHoleRadius, int count)
        {
            var random = new Random();
            var blackHoles = new List<BlackHole>();

            for (int i = 0; i < count; i++)
            {
                // Generate random angle
                double angle = random.NextDouble() * 2 * Math.PI;

                // Calculate position on the circle
                decimal x = innerRadius * (decimal)Math.Cos(angle);
                decimal y = innerRadius * (decimal)Math.Sin(angle);

                blackHoles.Add(new BlackHole(new Point(x, y), blackHoleRadius));
            }

            return blackHoles;
        }

        /// <summary>
        /// Checks if a point is inside a circle
        /// </summary>
        private static bool IsPointInCircle(Point point, Point center, decimal radius)
        {
            decimal dx = point.X - center.X;
            decimal dy = point.Y - center.Y;
            decimal distanceSquared = dx * dx + dy * dy;
            return distanceSquared <= radius * radius;
        }

    }
}
