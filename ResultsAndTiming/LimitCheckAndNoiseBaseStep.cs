// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.

namespace OpenTap.Plugins.Demo.ResultsAndTiming
{
    [Display("LimitCheckAndNoiseBase", Groups: new[] { "Demo", "Results And Timing" },
        Description: @"A base class that extends TestStepBase.\nUseful for implementing tests that want to have noise results with a limit checker\nUseful for generating data for use in the Results Viewer.")]
    public abstract class LimitCheckAndNoiseBaseStep : DemoBaseStep
    {
        [Display("Mean", "Mean value before adding noise.", "Results", Order: -3.1)]        
        public double Mean { get; set; }
        
        [Display("Noise Enabled", "Noise enabled.", "Results", Order: 1)]
        public bool NoiseEnabled { get; set; }
        
        [Display("Noise StdDev", "Standard deviation in values of noise.", "Results", Order: 2)]
        [EnabledIf("NoiseEnabled")]
        public double NoiseStdDev { get; set; }

        [Display("Enabled", "Limit Checking against limits.", "Limit Checking", Order: 1)]
        public bool LimitCheckingEnabled { get; set; }
        
        [Display("Lower Limit", "Lower limit for limit checking.", "Limit Checking", Order: 2)]
        [EnabledIf("LimitCheckingEnabled")]
        public double LowerLimit { get; set; }
        
        [Display("Upper Limit", "High Limit for limit checking.", "Limit Checking", Order: 3)]
        [EnabledIf("LimitCheckingEnabled")]
        public double UpperLimit { get; set; }

        protected LimitCheckAndNoiseBaseStep()
        {
            ResultName = "LimitCheckAndNoiseBase";
            
            NoiseEnabled = false;
            Mean = 20;
            NoiseStdDev = 3;

            LimitCheckingEnabled = false;
            LowerLimit = 5;
            UpperLimit = 30;
        }

        protected void CheckLimits(double value)
        {
            if (LimitCheckingEnabled)
            {
                if ((value < LowerLimit) || (value > UpperLimit))
                {
                    UpgradeVerdict(Verdict.Fail);
                }
                else
                {
                    UpgradeVerdict(Verdict.Pass);
                }
            }
            //else the Verdict is not changed. 
        }
    }
}
