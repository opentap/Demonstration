// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using OpenTap;

namespace OpenTap.Plugins.Demo.ResultsAndTiming
{
    [Display("DemoBaseStep", "A base class for implementing tests that return results.\nUseful for generating data for use in the Results Viewer.", 
        Groups: new[] { "Demo", "Results And Timing" })]
    public abstract class DemoBaseStep : TestStep
    {   
        [Display("Result Name", "Name of this result.", Group:"Results", Order: -3.3)]
        public string ResultName { get; set; }

        [Display("Point Count", "Number of points to generate.", Group: "Results", Order: -3.2)]
        public int PointCount { get; set; }

        protected DemoBaseStep()
        {
            ResultName = "DemoBaseStep";
            PointCount = 50;
        }

        public override void Run()
        {
        }
    }
}
