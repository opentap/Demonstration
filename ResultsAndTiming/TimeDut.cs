// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System.Diagnostics;
using System.Threading;

namespace OpenTap.Plugins.Demo.ResultsAndTiming
{
    [Display("Timing DUT", "Dummy DUT with configurable open, action and close times.\nUseful for demonstrating the Timing Analyzer."
        ,Groups: new[] { "Demo", "Results And Timing" })]
    public class TimeDut : Dut
    {
        #region Settings
        
        [Display("Open Time", "Seconds spent connecting.", Group: "Timing", Order: - 2)]
        [Unit("s")]
        public double OpenTimeSecs { get; set; }
        
        [Display("Close Time", "Seconds spent disconnecting.", Group: "Timing", Order: 0)]
        [Unit("s")]
        public double CloseTimeSecs { get; set; }
        
        [Display("Action Time", "Seconds spent in some activity.", Group: "Timing", Order: -1)]
        [Unit("s")]
        public double ActionTimeSecs { get; set; }


        #endregion

        /// <summary>
        /// Initializes a new instance of this DUT class.
        /// </summary>
        public TimeDut()
        {
            Name = "TimeDut";
            OpenTimeSecs = .1;
            CloseTimeSecs = .1;
            ActionTimeSecs = .1;
        }

        /// <summary>
        /// Opens a connection to the DUT represented by this class
        /// </summary>
        public override void Open()
        {
            Thread.Sleep((int) (OpenTimeSecs * 1000));
            base.Open();
        }

        /// <summary>
        /// Closes the connection made to the DUT represented by this class
        /// </summary>
        public override void Close()
        {
            Thread.Sleep((int)(CloseTimeSecs * 1000));
            base.Close();
        }

        public void Action(string name)
        {
            var sw = Stopwatch.StartNew();
            var sleepTimeMilliSeconds = (int)(ActionTimeSecs * 1000);
            TapThread.Sleep(sleepTimeMilliSeconds);
            Log.Debug(sw.Elapsed, name);
        }
    }
}
