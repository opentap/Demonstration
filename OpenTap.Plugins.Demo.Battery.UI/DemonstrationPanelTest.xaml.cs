using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Keysight.OpenTap.Gui;
using Keysight.OpenTap.Plugins.Csv;
using Keysight.OpenTap.Plugins.ResultListeners;
using Keysight.OpenTap.Wpf;

namespace OpenTap.Plugins.Demo.Battery.UI;

public partial class DemonstrationPanelTest : UserControl
{
    private readonly ITapDockContext _context;

    public DemonstrationPanelTest(ITapDockContext context)
    {
        _context = context;
        InitializeComponent();
    }

    private void LoadResources()
    {
        ComponentSettings.SetSettingsProfile("Bench", Path.GetFullPath(Path.Combine("Settings","Bench","Battery Demo")));
        InstrumentSettings.Current.Clear();
        InstrumentSettings.Current.Add(new PowerAnalyzer());
        InstrumentSettings.Current.Add(new TemperatureChamber());
        InstrumentSettings.Current.Save();
        DutSettings.Current.Clear();
        DutSettings.Current.Add(new BatteryDut());
        
        DutSettings.Current.Save();
        if (ResultSettings.Current.OfType<CsvResultListener>().Any() == false)
            ResultSettings.Current.Add(new CsvResultListener());

        if (ResultSettings.Current.OfType<SQLiteDatabase>().Any() == false)
        {
            ResultSettings.Current.Add(new SQLiteDatabase());
            ResultSettings.Current.Save();
        }
        
        InstrumentSettings.Current.Invalidate();
        DutSettings.Current.Invalidate();
        
    }
    
    private void LoadTestPlan()
    {
        var plan = new TestPlan();
        _context.Plan = plan;
        
        var tempSweep = new BasicSteps.SweepParameterStep();
        var temp = new SetTemperatureStep() { };
        var charge = new ChargeStep();
        var discharge = new DischargeStep();
        tempSweep.ChildTestSteps.Add(temp);
        tempSweep.ChildTestSteps.Add(charge);
        tempSweep.ChildTestSteps.Add(discharge);// set the sweep loop to being expanded.
        ChildItemVisibility.SetVisibility(tempSweep, ChildItemVisibility.Visibility.Visible);
        
        plan.ChildTestSteps.Add(tempSweep);
        
        TypeData.GetTypeData(temp)
            .GetMember(nameof(temp.Temperature))
            .Parameterize(tempSweep, temp, "Temperature");
        
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = -10.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 0.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 10.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 24.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 30.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 45.0}});
        
        plan.Save("Battery Demo.TapPlan");
        
        var testPlanGridType = TypeData.GetTypeData("Keysight.OpenTap.Gui.TestPlanPlugin");
        TapPanel.Focus(testPlanGridType);
        
        
    }
    
    private void OnRunTestPlan(object sender, RoutedEventArgs e)
    {
        _context.Run();
        var testPlanGridType = TypeData.GetTypeData("Keysight.OpenTap.Gui.TestPlanPlugin");
        TapPanel.Focus(testPlanGridType);
    }

    private void LoadViewPreset()
    {
        var destPath = Path.Combine(ViewPreset.PresetDir, "batterydemo.xml");
        var srcPath = Path.Combine("Packages", "Demonstration", "battery_demo_preset.xml");
        File.Delete(destPath);
        File.Copy(srcPath, destPath);

        ViewPreset.SelectPreset(Path.GetFileNameWithoutExtension(destPath), raiseEvent: true);
        
    }
    private void OnShowResultsViewer(object sender, RoutedEventArgs e)
    {
        var templatePath = Path.Combine("Packages", "Demonstration", "battery_demo.TapReport");
        var store = ResultSettings.Current.OfType<IResultStore>().FirstOrDefault();
        ResultsViewer.Open(store, Application.Current.MainWindow, templatePath, 1);
    }
    
    private void OnLoadDemo(object sender, RoutedEventArgs e)
    {
        var response = ShowMessage("Load Demonstration?",
            " Loading the battery test demonstration will cause the following changes:\n" +
            " - New bench profile: a DUT, a power analyzer and a temperature chamber.\n" +
            " - Added result listeners: Adding CSV and SQLite database result storage.\n"+
            " - New view preset: A view preset will be applied providing an optimized view.\n" +
            " - New test plan: A demonstration test plan will be loaded.\n\n" +
            
            "This can be undone by selecting the previous bench profile and view preset.",
            ["OK", "Cancel"]);
        if (response == "Cancel")
            return;
        LoadResources();
        LoadTestPlan();
        LoadViewPreset();
    }

    public class MessageBox : IDisplayAnnotation
    {
        string IDisplayAnnotation.Description { get; }
        string[] IDisplayAnnotation.Group { get; }
        
        double IDisplayAnnotation.Order { get; }
        bool IDisplayAnnotation.Collapsed { get; }
        
        
        public string Name { get; internal set; }
        
        [Layout(LayoutMode.FullRow)]
        [Browsable(true)]
        public string Message { get; internal set; }
        
        public bool ShowFilePath { get; internal set; }
        
        [EnabledIf(nameof(ShowFilePath), true, HideIfDisabled = true)]
        [FilePath(FilePathAttribute.BehaviorChoice.Open)]
        [Display ("File Path")]
        public string FilePath { get; set; }


        public string[] Options { get; internal set; } = ["OK"];
        
        [Layout(LayoutMode.FloatBottom|LayoutMode.FullRow)]
        [Submit]
        [AvailableValues(nameof(Options))]
        public string SelectedOption { get; set; }
    }

    string ShowMessage(string title, string message, string[] options = null)
    {
        var box = new MessageBox
        {
            Name = title, Message = message,
            Options = options ?? ["OK"],
            SelectedOption = options?.FirstOrDefault() ?? "OK"
        };
        UserInput.Request(box);
        return box.SelectedOption;
    }
}