﻿// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.

using System.Diagnostics;
using OpenTap;

namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Power Analyzer", "Simulated power analyzer instrument used for charge/discharge demo steps.", Groups: new[] { "Demo", "Battery Test" })]
    public class PowerAnalyzer : Instrument
    {
        #region Settings 
        [Display("Cell Size Factor", "A larger cell size will result in faster charging and discharging.")]               
        public double CellSizeFactor { get; set; }
        #endregion

        public PowerAnalyzer()
        {
            Name = "PSU";

            CellSizeFactor = 0.005;
            Rules.Add(() => (CellSizeFactor >= 0) && (CellSizeFactor <= .1), "CellSizeFactor must be >= 0 and <= .1", "Voltage");
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {
            base.Open();
            _voltage = 0;
            _cellVoltage = 2.7;
            Log.Info("Device PSU opened");
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

        public double MeasureCurrent()
        {
            UpdateCurrentAndVoltage();
            return _current;
        }

        public double MeasureVoltage()
        {
            UpdateCurrentAndVoltage();
            return _cellVoltage;
        }

        internal void Setup(double voltage, double current)
        {
            _voltage = voltage;
            _currentLimit = current;
            _current = current;
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
        private double _cellVoltage = 2.7;
        private double _current = 10;
        private double _currentLimit;
        Stopwatch _sw;
        private void UpdateCurrentAndVoltage()
        {
            if (_sw == null || !_sw.IsRunning)  // Only update if output is enabled
                return;

            // Generates a somewhat random curve that gradually approaches the limit.
            _current = (_currentLimit * (_voltage - _cellVoltage)*2 + RandomNumber.Generate()*_currentLimit/50);

            if (_current >= _currentLimit)
            {
                _current = _currentLimit;
            }
            else if (_current < 0-_currentLimit)
            { 
                _current = 0- _currentLimit;
            }

            _cellVoltage += CellSizeFactor * _current * _sw.Elapsed.TotalSeconds * 10;
            _sw.Restart();
        }
    }
}
