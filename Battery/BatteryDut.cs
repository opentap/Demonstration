namespace OpenTap.Plugins.Demo.Battery
{
    [Display("Battery", "This DUT represents the battery itself.")]
    public class BatteryDut : Dut
    {
        #region Settings

        [Display("Capacity", "A larger cell size will result in faster charging and discharging.")]
        [Unit("Ah")]
        public double Capacity { get; set; } = 0.3;

        [Display("Base Voltage", "The battery voltage when discharged.")]
        [Unit("V")]
        public double BaseVoltage { get; set; } = 3.0;
        
        [Display("Charged Voltage", "The battery voltage when charged.")]
        [Unit("V")]
        public double ChargedVoltage { get; set; } = 4.2;
        
        #endregion
        
        public BatteryModel Model { get; private set; }
        public BatteryDut()
        {
            Name = "Bat";
            Rules.Add(() => Capacity >= 0, "Capacity must be greater than 0", nameof(Capacity));
            Rules.Add(() => BaseVoltage >= 0, "Base Voltage must be greater than 0", nameof(BaseVoltage));
            Rules.Add(() => ChargedVoltage >= BaseVoltage, "Charted Voltage must be greater than 0", nameof(ChargedVoltage));
            Model = new BatteryModel();
        }

        public override void Open()
        {
            Model = new BatteryModel(initialCharge_Ah: 0.01, capacity_Ah: Capacity, chargedVoltage: ChargedVoltage, baseVoltage: BaseVoltage);
            base.Open();
        }
    }
}