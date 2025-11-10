using System.Windows;
using Keysight.OpenTap.Wpf;

namespace OpenTap.Plugins.Demo.Battery.UI;

[Display("Battery Demo", Group:"Demonstration", Description: "This is a demonstration panel for the battery test.")]
public class BatteryDemoPanel : IGettingStartedPanel
{
    
    public FrameworkElement CreateElement(ITapDockContext context)
    {
        return new DemonstrationPanelTest(context);
    }

    public double? DesiredWidth => 600;
    public double? DesiredHeight => 800;
}
