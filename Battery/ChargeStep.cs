// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System;
using OpenTap;

using System.Diagnostics;  // Use Platform infrastructure/core components (log,TestStep definition, etc)

namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Charge", "Simulates charging of a battery.", Groups: new[] { "Demo", "Battery Test" })]
    public class ChargeStep : SamplingStepBase
    {
        #region Settings
        [Display("Charge Current",Group: "Power Supply", Order:-1 )]
        [Unit("A")]
        public double Current { get; set; }
        [Display("Voltage", Group: "Power Supply", Order: -1)]
        [Unit("V")]
        public double Voltage { get; set; }
        
        [Display("Target Voltage", Group: "Power Supply", Order: -1)]
        [Unit("V")]
        public double TargetVoltage { get; set; }
        
        [Display("Charge Time", Group: "Output", Order: 0)]
        [Unit("s")]
        [Output]
        public double ChargeTime { get; private set; }
        
        
        #endregion

        public ChargeStep()
        {
            Voltage = 4.2;
            Current = 10;
            TargetVoltage = 4.1;
            Rules.Add(() => (Voltage >= 0) && (Voltage <= 10), "Voltage must be >= 0 and <= 10", nameof(Voltage));
            Rules.Add(() => (Current >= 0) && (Current <= 20), "Current must be >= 0 and <= 20", nameof(Current));
            Rules.Add(() => TargetVoltage < Voltage, "Target voltage must be less than the voltage", nameof(TargetVoltage));
        }

        public override void Run()
        {
            var sw = Stopwatch.StartNew();
            PowerAnalyzer.Setup(Voltage, Current, Dut.CellSizeFactor);
            PowerAnalyzer.EnableOutput();
            Log.Info("Charging at: " + Current + "A" + " Target Voltage: " + Voltage + "V");
            base.Run();  // Most of the work is being done here, with callbacks to this class.
            PowerAnalyzer.DisableOutput();
            ChargeTime = sw.Elapsed.TotalSeconds;
            UpgradeVerdict(Verdict.Pass);
        }

        protected override void WhileSampling()
        {
            while(PowerAnalyzer.MeasureVoltage() < TargetVoltage)
            {
                TapThread.Sleep(50);
            }
        }

        [Display("Charge")]
        public class ChargeResult
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
            barVoltage.LowerLimit = 2; //Cell voltage defined in PowerAnalyzer
            barVoltage.UpperLimit = 4.7;
            Log.Info("Voltage: " + barVoltage.GetBar(voltage));
            
            Results.Publish(new ChargeResult { SampleNo = sampleNo, Voltage = Math.Truncate(voltage * 100) / 100, Current = Math.Truncate(current * 100) / 100});
        }

    }

}
