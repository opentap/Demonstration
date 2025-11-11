// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.

using System;
using System.Diagnostics;
using OpenTap;

namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Power Analyzer", "Simulated power analyzer instrument used for charge/discharge demo steps.", Groups: new[] { "Demo", "Battery Test" })]
    public class PowerAnalyzer : Instrument
    {
        public PowerAnalyzer()
        {
            Name = "PSU";       
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {
            base.Open();
            _voltage = 0;
            Log.Info("Device PSU opened");
            _sw = new Stopwatch();
            _sw.Start();
            
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            Log.Info("Device PSU closed");

            if (_sw != null)
                _sw.Stop();

            base.Close();
        }

        public (double voltage, double current) Measure(BatteryDut dut)
        {
            if(_sw.ElapsedMilliseconds > 1)
            {
                dut.Model.Update(_voltage, _sw.Elapsed.TotalSeconds, TemperatureChamber.Temperature, _currentLimit);
                _sw.Restart();
            }
            return (dut.Model.Voc, dut.Model.Current_A);
        }

        internal void Setup(double voltage, double current)
        {
            _voltage = voltage;
            _currentLimit = current;
        }

        internal void EnableOutput()
        {
            if (_sw == null || !_sw.IsRunning)
                _sw = Stopwatch.StartNew();
        }

        internal void DisableOutput()
        {
            if (_sw != null)
                _sw.Stop();
        }

        private double _voltage;
        Stopwatch _sw;
        private double _currentLimit;
    }
}
