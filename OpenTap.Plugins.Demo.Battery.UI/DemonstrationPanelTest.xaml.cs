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
    private const string presetName = "Battery Demo";
    private readonly ITapDockContext _context;

    public DemonstrationPanelTest(ITapDockContext context)
    {
        _context = context;
        InitializeComponent();
    }

    private void OnLoadInstrumentsClicked(object sender, RoutedEventArgs e)
    {
        var response = ShowMessage("Loaded Resources", "Loaded a DUT, a power analyzer and a temperature chamber.\n\n" +
                                        "This will cause the Battery Demo bench profile to be loaded.\n" +
                                        "To undo this change the bench profile in your bench settings.",
            ["OK", "Cancel"]);
        if (response == "Cancel")
            return;
        ComponentSettings.SetSettingsProfile("Bench", "Settings/Bench/Battery Demo");
        InstrumentSettings.Current.Clear();
        InstrumentSettings.Current.Add(new PowerAnalyzer());
        InstrumentSettings.Current.Add(new TemperatureChamber());
        DutSettings.Current.Clear();
        DutSettings.Current.Add(new BatteryDut());
        if(ResultSettings.Current.OfType<CsvResultListener>().Any() == false)
            ResultSettings.Current.Add(new CsvResultListener());
        if(ResultSettings.Current.OfType<SQLiteDatabase>().Any() == false)
            ResultSettings.Current.Add(new SQLiteDatabase());

    }
    
    private void OnLoadTestPlan(object sender, RoutedEventArgs e)
    {
        var plan = new TestPlan();
        _context.Plan = plan;
        
        var tempSweep = new BasicSteps.SweepParameterStep();
        var temp = new SetTemperatureStep() { };
        var charge = new ChargeStep();
        var discharge = new DischargeStep();
        plan.ChildTestSteps.Add(tempSweep);
        tempSweep.ChildTestSteps.Add(temp);
        tempSweep.ChildTestSteps.Add(charge);
        tempSweep.ChildTestSteps.Add(discharge);
        
        TypeData.GetTypeData(temp)
            .GetMember(nameof(temp.Temperature))
            .Parameterize(tempSweep, temp, "Temperature");
        
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = -10.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 0.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 10.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 24.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 30.0}});
        tempSweep.SweepValues.Add(new BasicSteps.SweepRow(){Values = {["Temperature"] = 45.0}});
        
        
    }
    
    private void OnRunTestPlan(object sender, RoutedEventArgs e)
    {
        _context.Run();
        var testPlanGridType = TypeData.GetTypeData("Keysight.OpenTap.Gui.TestPlanPlugin");
        TapPanel.Focus(testPlanGridType);
    }

    private void OnLoadPreset(object sender, RoutedEventArgs e)
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