# Test Step Details

## Set Temperature Test Step

The **Set Temperature** test step simulates the setting of temperature and humidity on a temperature chamber. It has three settings:

- **Chamber** — Required. Must reference a previously defined instrument of type *Temperature Chamber*.
- **Humidity** — Configurable, but not used in the simulation. 
- **Temperature** — Used in the simulation. In most cases, this setting would be swept by a parent Sweep Loop step.

## Charge Test Step

The **Charge** test step simulates:

- Setting the voltage, current and measurement interval on a power supply 
- Measuring the charge time via a previously defined Power Analyzer 

The **Charge Time** setting is an output whose value is calculated by the test step. It is typically used by a following test step that has an input with a matching type.

## Discharge Test Step

The **Discharge** test step simulates:

- Setting the voltage, current and measurement interval on a power supply 
- Measuring the discharge time via a previously defined Power Analyzer 

The **Discharge Time** setting is an output whose value is calculated by the test step. It is typically used by a following test step that has an input with a matching type.

## Rating Test Step

The **Rating** test step has two input variables (*Charge Time* and *Discharge Time*) that should be related to corresponding output variables from prior test steps. The **Limits** group allows users to set the lower limits for *Best* and *Better* total charge and discharge time. The test step calculates a rating, which can be shown in the Test Plan panel.
