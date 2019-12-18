// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System.Timers;
using OpenTap;

namespace OpenTap.Plugins.Demo.Battery
{
    public abstract class SamplingStepBase : TestStep
    {
        private int _sampleNo;
        
        [Display("Measure Interval", "The time between measurements", Group: "Measurements", Order: -50)]
        [Unit("s")]
        public double MeasurementInterval { get; set; }

        [Display("Power Analyzer", Group: "Resources", Order: -100)]
        public PowerAnalyzer PowerAnalyzer { get; set; }
        
        public SamplingStepBase()
        {
            MeasurementInterval = .2;
        }

        public override void Run()
        {
            _sampleNo = 0;
            Timer timer = new Timer((int)(MeasurementInterval * 1000));
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            try
            {
                // Sleep, while the timer thread generates data.
                // Will stop when the charge/discharge gets within margin
                WhileSampling();
                RunChildSteps(); //If step has child steps.
            }
            finally
            {
                timer.Stop();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            double voltage = PowerAnalyzer.MeasureVoltage();
            double current = PowerAnalyzer.MeasureCurrent();
            OnSample(voltage, current, _sampleNo++);
        }

        protected abstract void WhileSampling();
        
        protected abstract void OnSample(double voltage, double current, int sampleNo);
    }
}
