# Test Step Details

## Ramp Result

This test step generates a ramp with configurable number of results. It has two settings:

- **Result Name** - the name for the result that will appear in the *Result Viewer* as *Title* under *Chart Settings*.
- **Point Count** - the number of points to be generated for the graph that appears in the *Result Viewer*.

This step is useful for demonstrating the **Results Viewer**.

## Sine Result

This test step generates a sine wave with settable amplitude and point count. This test step has multiple settings:

- *Results:*
    - **Result Name** - the name of the result that will appear in the *Result Viewer* as *Title* under *Chart Settings*.
    - **Point Count** - the number of points to be generated for the graph that appears in the *Result Viewer*.
    - **Amplitude PP** - the Peak to Peak amplitude, around the mean, without adding noise. 
    - **Mean** - the mean value before adding noise.
    - **Noise Enabled** - enable or disable noise. Disabled by default.
    - **Noise StdDev** - the standard deviation for values of noise.

- *Limit Checking:*
    - **Enabled** - enable or disable limit checking. Disabled by default.
    - **Lower Limit** - the lower limit for limit checking.
    - **Upper Limit** - the upper limit for limit checking.

This step is useful for demonstrating the **Results Viewer**.

## Time Activity

This is a dummy test step that allows to configure times for PrePlanRun, Run and PostPlanRun. It has the following settings:

- *Resources:*
    - **Dut** - to specify a device under test that the test step can use.
    - **Instrument** - to specify an instrument that the test step can use.
- *Results:*
    - **Result Name** - the name of the result.
- *PrePlanRun:*
    - **Mean Time** - the mean of the seconds spent in PrePlanRun.
- *Run Time:*
    - **Mean Time** - the mean of the seconds spent in Run.
    - **Time StdDev** - the standard deviation of seconds spent in Run.
- *PostPlanRun:*
    - **Mean Time** - the mean of the seconds spent in PostPlanRun.

This step is useful for demonstrating the **Timing Analyzer**.

## Time Resource Activity

This is a dummy test step which uses resources (DUT and Instrument) and allows to configure times for PrePlanRun and PostPlanRun. The step hast the following settings:

- *Resources:*
    - **Dut** - to specify a device under test that the test step can use.
    - **Instrument** - to specify an instrument that the test step can use.
- *Results:*
    - **Result Name** - the name of the result.
- *PrePlanRun:*
    - **Mean Time** - the mean of the seconds spent in PrePlanRun.
- *PostPlanRun:*
    - **Mean Time** - the mean of the seconds spent in PostPlanRun.

This step is useful for demonstrating the **Timing Analyzer**.