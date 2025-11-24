// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.

using System.Diagnostics;
namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Set Temperature", "Configure the temperature chamber.", Groups: new[] {"Demo", "Battery Test" })]
    public class SetTemperatureStep : TestStep
    {
        #region Settings
        [Display("Chamber", Group: "Resources", Order: 1)]
        public TemperatureChamber Chamber { get; set; }

        
        [Display("Temperature", Group: "Configuration", Order: 10)]
        [Unit("°C")]
        public double Temperature { get; set; }
        
        [Display("Humidity", "Does not impact this simulation", Group: "Configuration", Order: 11)]
        [Unit("%")]
        public double Humidity { get; set; }
        #endregion

        public SetTemperatureStep()
        {
            Temperature = 25;
            Humidity = 50;
            Rules.Add(() => (Temperature >= -10) && (Temperature <= 50), "Temperature must be > -10 and < 50", "Temperature");
            Rules.Add(() => (Humidity >= 0) && (Humidity <= 100), "Humidity must be >= 0 and <= 100", "Humidity");
        }

        public override void Run()
        {
            Log.Info("Temperature set to: " + Temperature + " °C");
            bool waiting = true;
            TapThread.Start(() =>
            {
                while (waiting)
                {
                    Log.Info($"Current temperature: {TemperatureChamber.Temperature:F1} °C");
                    TapThread.Sleep(1000);
                }

            });
            Chamber.SetTarget(Temperature, Humidity);
            var sw = Stopwatch.StartNew();
            
            Chamber.WaitForConditions();
            waiting = false;
            
            Log.Info(sw, "Temperature reached: " + Temperature + " °C");
        }
    }
}
