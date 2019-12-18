// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System;

namespace OpenTap.Plugins.Demo.Battery
{
    static class RandomNumber
    {
        static Random rnd = new Random();

        public static double Generate()
        {
            return rnd.NextDouble();
        }

        public static double Generate(double min, double max)
        {
            return min + rnd.NextDouble() * (max-min);
        }
    }
}
