// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System;
using System.Collections.Generic;
using OpenTap;

namespace OpenTap.Plugins.Demo.ResultsAndTiming
{
    [Display("Sine Result", Groups: new[] { "Demo", "Results And Timing" },
        Description: "Generates a sine wave with settable amplitude and point count.\nUseful for demonstrating the Results Viewer.")]
    public class SineResultsStep : LimitCheckAndNoiseBaseStep
    {
        #region Settings
        
        [Display("Amplitude PP", "The Peak to Peak amplitude, around the mean, without adding noise.", "Results", Order:-3.1)]
        public double AmplitudePp { get; set; }

        #endregion

        public SineResultsStep()
        {
            ResultName = "SineResults";
            AmplitudePp = 20;
        }
        
        public override void Run()
        {   
            var rnd = new Random();
            var values = new double[PointCount];
            var pointIndices = new int[PointCount];
            for (var i = 0; i < PointCount; i++)
            {
                var radians = i * (2*Math.PI/PointCount);
                var pureValue = Mean + AmplitudePp/2 * Math.Sin(radians);
                var randomAddon = NoiseEnabled ? RandomValue.CalcValue(0, NoiseStdDev) : 0;
                var value = pureValue + randomAddon;

                CheckLimits(value);
                values[i] = value;
                pointIndices[i] = i;
            }

            Results.PublishTable(ResultName, new List<string> { "PointIndex", "Value"}, pointIndices, values);
        }
    }
}
