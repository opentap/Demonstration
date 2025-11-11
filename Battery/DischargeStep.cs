// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System;
using System.Diagnostics;  // Use Platform infrastructure/core components (log,TestStep definition, etc)
using OpenTap;

namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Discharge", "Simulates discharging of a battery.", Groups: new[] { "Demo", "Battery Test" })]
    public class DischargeStep : SamplingStepBase
    {
        #region Settings
        [Display("Discharge Current", Group: "Power Supply", Order: -1)]
        [Unit("A")]
        public double Current { get; set; }
        [Display("Voltage", Group: "Power Supply", Order: -1)]
        [Unit("V")]
        public double Voltage { get; set; }
        
        [Display("Target Voltage", Group: "Power Supply", Order: -1)]
        [Unit("V")]
        public double TargetVoltage { get; set; }

        
        [Display("Discharge Time", Group: "Output", Order: 0)]
        [Unit("s")]
        [Output]
        public double DischargeTime { get; private set; }

        #endregion

        public DischargeStep()
        {
            Voltage = 2.2;
            Current = 5;
            TargetVoltage = 3.1;
            Rules.Add(() => (Voltage >= 0) && (Voltage <= 10), "Voltage must be >= 0 and <= 10", "Voltage");
            Rules.Add(() => (Current >= 0) && (Current <= 20), "Current must be >= 0 and <= 20", "Current");
            Rules.Add(() => (TargetVoltage > Voltage), "Target Voltage must be greater than the voltage.", "TargetCellVoltageMargin");
        }

        public override void Run()
        {
            var sw = Stopwatch.StartNew();
            PowerAnalyzer.Setup(Voltage, Current);
            PowerAnalyzer.EnableOutput();
            Log.Info("Discharging at: " + Current + "A" + " Target Voltage: " + Voltage + "V");
            base.Run();
            PowerAnalyzer.DisableOutput();
            DischargeTime = sw.Elapsed.TotalSeconds;
            UpgradeVerdict(Verdict.Pass);
        }

        protected override void WhileSampling()
        {
            while(Dut.Model.Voc > TargetVoltage)
            {
                TapThread.Sleep(50);
            }
        }

        [Display("Discharge")]
        public class DishargeResult
        {
            [Display("Sample Number")]
            public int SampleNo { get; set; }
            [Display("Voltage")]
            public double Voltage { get; set; }
            [Display("Current")]
            public double Current { get; set; }
        }

        protected override void OnSample(double voltage, double current, int sampleNo)
        {
            TraceBar barVoltage = new TraceBar();
            barVoltage.LowerLimit = 2;
            barVoltage.UpperLimit = 4.7;
            Log.Info("Voltage: " + barVoltage.GetBar(voltage));
            
            Results.Publish(new DishargeResult { SampleNo = sampleNo, Voltage = Math.Truncate(voltage * 100) / 100, Current = Math.Truncate(current * 100) / 100 });
        }

    }
}
