// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.

using System;

namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Temperature Chamber", "Simulated temperature chamber instrument used for SetTemperature demo step.", Groups: new[] { "Demo", "Battery Test" })]
    public class TemperatureChamber : Instrument
    {
        private double humidityTarget = 50;
        private double temperatureTarget = 50;
        public static double Temperature = 25;
        public static double Humidity = 25;
        
        public TemperatureChamber()
        {
            Name = "TEMP";
        }

        /// <summary>
        /// Open procedure for the instrument.
        /// </summary>
        public override void Open()
        {
            base.Open();
            Log.Info("Device TEMP opened", this.ToString());
        }

        /// <summary>
        /// Close procedure for the instrument.
        /// </summary>
        public override void Close()
        {
            Log.Info("Device TEMP closed");
            base.Close();
        }

        public void SetTarget(double temperature, double humidity)
        {
            this.temperatureTarget = temperature;
            this.humidityTarget = humidity;
        }

        public void WaitForConditions()
        {
            var rnd = new Random();
            if (temperatureTarget > Temperature)
            {
                Log.Debug("Heating...");
                while (temperatureTarget > Temperature)
                {
                    // heating up.
                    Temperature += rnd.NextDouble() * 0.02 + 0.05;
                    TapThread.Sleep(10);
                    OnActivity();
                }
            }else if (temperatureTarget < Temperature)
            {
                Log.Debug("Cooling...");
                while (temperatureTarget < Temperature)
                {
                    
                    Temperature -= (rnd.NextDouble() * 0.02 + 0.05);
                    TapThread.Sleep(10);
                    OnActivity();
                }
                
            }

            Temperature = temperatureTarget;
        }
    }
}
