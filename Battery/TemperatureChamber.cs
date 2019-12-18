// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using OpenTap;

namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Temperature Chamber", "Simulated temperature chamber instrument used for SetTemperature demo step.", Groups: new[] { "Demo", "Battery Test" })]
    public class TemperatureChamber : Instrument
    {
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
    }
}
