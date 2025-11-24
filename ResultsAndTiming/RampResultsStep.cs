// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System.Collections.Generic;

namespace OpenTap.Plugins.Demo.ResultsAndTiming
{
    [Display("Ramp Result", Groups: new[] { "Demo", "Results And Timing" },
        Description: "Generates a ramp with configurable number of results.\nUseful for demonstrating the Results Viewer.")]
    public class RampResultsStep : DemoBaseStep
    {
        public RampResultsStep()
        {
            PointCount = 50;
            ResultName = "RampResults";
        }

        public override void Run()
        {
            var pointIndices = new int[PointCount];
            var values = new double[PointCount];
            for (var i = 0; i < PointCount; i++)
            {
                pointIndices[i] = i;
                values[i] = i;
            }
            Results.PublishTable(ResultName, new List<string> { "PointIndex", "Value" }, pointIndices, values);
        }
    }
}
