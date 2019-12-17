using System;

namespace OpenTap.Plugins.Demo.ResultsAndTiming
{
    
    /// <summary>   A random value. </summary>
    internal class RandomValue
    {
        private static readonly Random RandomSeed = new Random();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculates a random value within a standard deviation. </summary>
        ///
        /// <param name="mean">     The mean. </param>
        /// <param name="stdDev">   The standard deviation. </param>
        ///
        /// <returns>   The calculated value. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double CalcValue(double mean, double stdDev)
        {
            var u1 = RandomSeed.NextDouble();
            var u2 = RandomSeed.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);
            var randNormal = mean + stdDev * randStdNormal;

            return randNormal;
        }

        public static double CalcValue(double stdDev)
        {
            return CalcValue(0, stdDev);
        }
    }
}
