// Copyright:   Copyright 2016 Keysight Technologies
//               You have a royalty-free right to use, modify, reproduce and distribute
//               the sample application files (and/or any modified version) in any way
//               you find useful, provided that you agree that Keysight Technologies has no
//               warranty, obligations or liability for any sample application files.
using System.ComponentModel;
using System.Xml.Serialization;
using OpenTap;

namespace OpenTap.Plugins.Demo.Battery
{
    
    [Display("Rating", "Calculate rating of a battery based on total charge and discharge time.", 
        Groups: new[] { "Demo", "Battery Test" })]
    public class RatingStep : TestStep
    {
        public enum Rating
        {
            Best,
            Better,
            Poor, 
            Unknown
        }
        [Display("Charge Time", Group: "Inputs")]
        [Unit("s")]
        public Input<double> ChargeTime { get; set; }

        [Display("Discharge Time", Group: "Inputs")]
        [Unit("s")]
        public Input<double> DischargeTime { get; set; }

        [ColumnDisplayName("Rating")]
        [Display("Rating")]
        [XmlIgnore]
        public Rating CalculatedRating { get; private set; }
        
        [Unit("s")]
        [Display("Rating Best", "Min total time for Best rating", "Limits")]
        public double MinBestLimit { get; set; }
        
        [Display("Rating Better", "Min total time for Better rating.", "Limits")]
        [Unit("s")]
        public double MinBetterLimit { get; set; }
        
        [Display("Total Charge and Discharge Time", Group: "Results")]
        [Browsable(true)]
        [XmlIgnore]
        [Unit("s")]
        public double TotalTime { get; private set; }

        public RatingStep()
        {
            MinBestLimit = 6.5;
            MinBetterLimit = 7;
            CalculatedRating = Rating.Unknown;

            ChargeTime = new Input<double>();            
            DischargeTime = new Input<double>();

            Rules.Add(() => ChargeTime?.Property != null, "Input for Charge Time needs to be set.", nameof(ChargeTime));
            Rules.Add(() => DischargeTime?.Property != null, "Input for Discharge Time needs to be set.", nameof(DischargeTime));
        }

        [Display("Rating")]
        public class RatingResult
        {
            [Display("Charge Time")]
            public double ChargeTime { get; set; }
            [Display("Discharge Time")]
            public double DischargeTime { get; set; }
            [Display("Total Time")]
            public double TotalTime { get; set; }
            [Display("Rating Value")]
            public Rating Rating { get; set; }
            [Display("Rating Name")]
            public string RatingName { get; set; }
        }

        public override void Run()
        {
            TotalTime = ChargeTime.Value + DischargeTime.Value;

            if (TotalTime < MinBestLimit)
            {
                CalculatedRating = Rating.Best;
            }
            else if (TotalTime < MinBetterLimit)
            {
                CalculatedRating = Rating.Better;
            }
            else
            {
                CalculatedRating = Rating.Poor;
            }

            string ratingName = CalculatedRating.ToString();

            Results.Publish(new RatingResult { ChargeTime = ChargeTime.Value, DischargeTime = DischargeTime.Value,
                TotalTime = TotalTime, Rating = CalculatedRating , RatingName = ratingName});
        }
    }
}
