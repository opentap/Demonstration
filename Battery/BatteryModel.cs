using System;

namespace OpenTap.Plugins.Demo.Battery
{
    /// <summary>
    /// This is a relatively inaccurate physical model of a Lithium-ion-like battery. 
    /// </summary>
    class BatteryModel
    {
        // --- Physical constants ---
        private const double Rgas = 8.314;    // J/mol·K
        private const double Tref = 298.15;   // 25°C in Kelvin
    
        // --- Simulation 
        private double baseVoltage = 3.0;
        private double chargedVoltage = 4.2;

        // --- Nominal parameters ---
        private readonly double nominalCapacity;  // Ah
        private readonly double R25;              // Internal resistance (Ohm) at 25°C
        private readonly double beta;             // Temperature coefficient for resistance
        private readonly double Ea;               // Activation energy (J/mol)
        private readonly double selfDischarge25;  // Self-discharge rate at 25°C (fraction/min)

        // --- State variables ---
        public double Charge_Ah { get; private set; }   // Current stored charge
        public double Voltage_V { get; private set; }   // Terminal voltage
        public double Current_A { get; private set; }   // Charging (+) or discharging (-)
        public double Voc { get; private set; }         // Voltage open-circuit
        public double SOC { get; private set; }         // State of Charge (0–1)
        public double CoulombicEfficiency { get; private set; }

        public BatteryModel(
            double capacity_Ah = 3.0,
            double initialCharge_Ah = 1.5,
            double internalResistance25 = 0.05,
            double temperatureCoeff = 0.04,
            double activationEnergy = 35000.0,
            double selfDischargeRate25 = 0.0001,
            double baseVoltage = 3.0,
            double chargedVoltage = 4.2)
        {
            nominalCapacity = capacity_Ah;
            Charge_Ah = initialCharge_Ah;
            R25 = internalResistance25;
            beta = temperatureCoeff;
            Ea = activationEnergy;
            selfDischarge25 = selfDischargeRate25;
            this.baseVoltage = baseVoltage;
            this.chargedVoltage = chargedVoltage;
        
            double capacity = nominalCapacity * 1;
            SOC = Charge_Ah / capacity;
            SOC = Math.Min(Math.Max(SOC, 0.0), 1.0);
            Voc = baseVoltage + (chargedVoltage - baseVoltage) * SOC + 0.05 * Math.Sin(5 * SOC);
        }

        /// <summary>
        /// Update the battery state given an applied terminal voltage, temperature, and timestep.
        /// </summary>
        /// <param name="appliedVoltage">Applied terminal voltage (V)</param>
        /// <param name="dt_min">Timestep (minutes)</param>
        /// <param name="temperature_C">Cell temperature (°C)</param>
        /// <param name="current_limit">Current limited by the generator. </param>
        public void Update(double appliedVoltage, double dt_min, double temperature_C, double current_limit)
        {
            double T_K = temperature_C + 273.15;
            
            // external resistance
            double R_external = 0.02;
            
            // --- Temperature-dependent parameters ---
            double R_internal = R25 * Math.Exp(beta * (25 - temperature_C));
            double capacity = nominalCapacity * (1 - 0.002 * Math.Abs(temperature_C - 25));
            double k_self = selfDischarge25 * Math.Exp(0.05 * (temperature_C - 25));

            // --- Compute open-circuit voltage based on SOC ---
            SOC = Charge_Ah / capacity;
            SOC = Math.Min(Math.Max(SOC, 0.0), 1.0);
            Voc = baseVoltage + (chargedVoltage - baseVoltage) * SOC + 0.05 * Math.Sin(5 * SOC);

            // --- Solve current from voltage equation ---
            // I is clamped by +/- current_limit. 
            // appliedVoltage = Voc - I * R_internal
            Current_A = Math.Max(Math.Min((Voc- appliedVoltage) / (R_internal + R_external), current_limit), -current_limit);

            // --- Temperature-dependent efficiency (Arrhenius relation) ---
            CoulombicEfficiency = Math.Exp(-Ea / Rgas * (1.0 / T_K - 1.0 / Tref));
            CoulombicEfficiency = Math.Min(Math.Max(CoulombicEfficiency, 0.7), 1.0);

            // --- Effective current (charging losses) ---
            double effectiveCurrent = Current_A;
            if (Current_A > 0) // Charging
                effectiveCurrent *= CoulombicEfficiency;

            // --- Update charge (Ah) ---
            Charge_Ah += -effectiveCurrent * dt_min / 60.0;

            // --- Apply self-discharge ---
            Charge_Ah -= Charge_Ah * k_self * dt_min;

            // --- Clamp charge ---
            Charge_Ah = Math.Min(Math.Max(Charge_Ah, 0.0), capacity);

            // --- Update terminal voltage ---
            Voltage_V = appliedVoltage;
        }
    }
}