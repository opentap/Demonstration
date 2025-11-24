// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenTap.Plugins.Demo.ResultsAndTiming;


namespace  OpenTap.Plugins.Example.ResultsAndTiming
{
    [Display("Time Resource Activity", "Dummy step using resources (DUT and instrument) with configurable times for PrePlanRun and PostPlanRun.\nUseful for demonstrating the Timing Analyzer.",
        Groups: new[] { "Demo", "Results And Timing" } )]
    public class TimeResourceActivityStep : TestStep
    {
        #region Settings

        [Display("Result Name", "Name of this result.", Group: "Results", Order: -5)]
        public string ResultName { get; set; }

        [Display("DUT", Group: "Resources")]
        public TimeDut MyDut { get; set; }

        [Display("Instrument", Group: "Resources")]
        public TimeInst MyInstrument { get; set; }
        

        [Display("Mean Time", Group: "PrePlanRun", Description: "Mean seconds spent in PrePlanRun", Order: -3)]
        [Unit("s")]
        public double PrePlanRunTimeMeanSecs { get; set; }
        
        [Display("Mean Time", Group: "PostPlanRun", Description: "Mean seconds spent in PostPlanRun", Order: -1)]
        [Unit("s")]
        public double PostPlanRunTimeMeanSecs { get; set; }

        #endregion
        public TimeResourceActivityStep()
        {   
            PrePlanRunTimeMeanSecs = 0;  
            PostPlanRunTimeMeanSecs = 0;
            ResultName = "TimeResourceActivity";
        }

        public override void PrePlanRun()
        {   
            base.PrePlanRun();
            var sleepTimeSecs = PrePlanRunTimeMeanSecs;
            Thread.Sleep((int) (1000 * sleepTimeSecs));
        }
        
        public override void Run()
        {
            var sw = Stopwatch.StartNew();
            
            // Ask the Duts and Insts to do something.
            MyDut.Action("Action1");
            MyInstrument.Action("Action1");
            MyDut.Action("Action2");
            MyDut.Action("Action3");
            MyInstrument.Action("Action2");
            MyInstrument.ActionTimeSecs = .01;
            MyInstrument.Action("Action3");
            
            RunChildSteps(); //If step has child steps.
            Results.Publish(ResultName, new List<string> {"Value"}, sw.Elapsed.TotalSeconds);
        }

        public override void PostPlanRun()
        {
            var sleepTimeSecs = PostPlanRunTimeMeanSecs;
            Thread.Sleep((int) (1000 * sleepTimeSecs));
        }
    }
}
