// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System.Collections.Generic;
using System.Threading;
using OpenTap;

namespace OpenTap.Plugins.Demo.ResultsAndTiming
{
    [Display("Time Activity", "Dummy step with configurable times for PrePlanRun, Run, and PostPlanRun.\nUseful for demonstrating the Timing Analyzer.",
        Groups: new[] { "Demo", "Results And Timing" })]
    public class TimeActivityStep : TestStep
    {
        #region Settings
        [Display("Result Name", "Name of this result.", Group: "Results", Order: -5)]
        public string ResultName { get; set; }
        
        [Display("Dut", Group: "Resources")]
        public TimeDut MyDut { get; set; }

        [Display("Instrument", Group: "Resources")]
        public TimeInst MyInstrument { get; set; }
        
        [Display("Mean Time", Group: "PrePlanRun", Description: "Mean seconds spent in PrePlanRun", Order: -3)]
        [Unit("s")]
        public double PrePlanRunTimeMeanSecs { get; set; }

        [Display("Mean Time", Group: "RunTime", Description: "Mean seconds spent in Run", Order: -2)]
        [Unit("s")]
        public double RunTimeMeanSecs { get; set; }

        [Display("Time StdDev", Group: "RunTime", Description: "Std Dev seconds spent in Run", Order: -2)]
        [Unit("s")]
        public double RunTimeStdDevSecs { get; set; }
        
        [Display("Mean Time", Group: "PostPlanRun", Description: "Mean seconds spent in PostPlanRun", Order: -1)]
        [Unit("s")]
        public double PostPlanRunTimeMeanSecs { get; set; }

        #endregion
        public TimeActivityStep()
        {   
            PrePlanRunTimeMeanSecs = 0;  

            RunTimeMeanSecs = .1;  
            RunTimeStdDevSecs = 0;
            
            PostPlanRunTimeMeanSecs = 0;

            ResultName = "TimeActivity";
        }


        public override void PrePlanRun()
        {   
            base.PrePlanRun();
            var sleepTimeSecs = PrePlanRunTimeMeanSecs;
            Thread.Sleep((int) (1000 * sleepTimeSecs));
        }

        public override void Run()
        {
            var stepDurationSecs = RandomValue.CalcValue(RunTimeMeanSecs, RunTimeStdDevSecs);
            stepDurationSecs = stepDurationSecs > 0 ? stepDurationSecs : 0;

            TapThread.Sleep((int)(1000 * stepDurationSecs));

            RunChildSteps(); //If step has child steps.
            
            Results.Publish(ResultName, new List<string> {"Value"}, stepDurationSecs);

            UpgradeVerdict(Verdict.NotSet);
        }

        public override void PostPlanRun()
        {
            var sleepTimeSecs = PostPlanRunTimeMeanSecs;
            Thread.Sleep((int)(1000 * sleepTimeSecs));
        }
    }
}
