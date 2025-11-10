namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Battery", "This DUT represents the battery itself.")]
    public class BatteryDut : Dut
    {
        #region Settings 
        [Display("Cell Size Factor", "A larger cell size will result in faster charging and discharging.")]               
        public double CellSizeFactor { get; set; }
        #endregion
        public BatteryDut()
        {
            Name = "Bat";

            CellSizeFactor = 0.005;
            Rules.Add(() => (CellSizeFactor >= 0) && (CellSizeFactor <= .1), "CellSizeFactor must be >= 0 and <= .1", "Voltage");
        }
    }
}