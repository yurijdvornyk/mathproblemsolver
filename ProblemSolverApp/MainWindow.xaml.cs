using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProblemLibrary;
using ProblemSolverApp.Windows;
using ProblemSolverApp.Classes;
using System.Reflection;
using System.Globalization;
using ProblemSolverApp.Classes.CustomLogger;
using Microsoft.Win32;
using System.Diagnostics;

namespace ProblemSolverApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Logger = CustomLogger.GetInstance();
            terminal.Logger = Logger;

            try
            {
                Logger.LogInfo("Session started.");
                var sep = System.IO.Path.DirectorySeparatorChar.ToString();
                ProblemPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + sep + "custom";
                _ProblemManager = ProblemManager.GetInstance();
                _ProblemManager.RepositoryPath = ProblemPath;
                Logger.LogInfo("Loading problems...");

                var builtInLibs = AppDomain.CurrentDomain.GetAssemblies().ToList();
                var builtInLibsNames = new List<string>();
                foreach (var i in builtInLibs)
                {
                    builtInLibsNames.Add(i.FullName);
                }
                _ProblemManager.Load(builtInLibsNames);


                problemDataControl._ProblemManager = _ProblemManager;

                if (_ProblemManager.ProblemFullInfoList.Count == 0)
                {
                    Logger.LogSuccess("Problems path is scanned successfully, but there no uploaded problems now.");
                }
                else
                {
                    Logger.LogSuccess("Problems loaded. There are " + _ProblemManager.ProblemFullInfoList.Count + " problems found.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("There were some errors while loading problems. Details:\n" + ex.Message);
            }
        }

        public string ProblemPath { get; set; }

        #region ProblemManager

        public ProblemManager _ProblemManager
        {
            get { return (ProblemManager)GetValue(_ProblemManagerProperty); }
            set { SetValue(_ProblemManagerProperty, value); }
        }

        public static readonly DependencyProperty _ProblemManagerProperty =
            DependencyProperty.Register("_ProblemManager", typeof(ProblemManager), typeof(MainWindow), new PropertyMetadata(null));

        #endregion

        #region Terminal

        public CustomLogger Logger { get; set; }

        #endregion

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            string name = string.Empty;
            try
            {
                var problem = _ProblemManager.GetProblem(problemDataControl.CurrentProblem);
                name = problem.Name;
                problem.Solve();
                problemResults.CurrentProblem = problem;
                problemResults.UpdateResults();

                Logger.LogSuccess(problem.Name + ": Problem calculated successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(name + ": There sers some errors while calculating the problem. Details:\n" + ex.Message);
            }
        }

        private void btnProblemManager_Click(object sender, RoutedEventArgs e)
        {
            ProblemRepositoryWindow pmWin = new ProblemRepositoryWindow(Logger, ProblemPath);
            Logger.LogInfo("Open problem repository.");
            pmWin.Show();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWin = new SettingsWindow();
            Logger.LogInfo("Open application settings.");
            settingsWin.Show();
        }

        private void btnExportProblemTex_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            var problem = problemDataControl.CurrentProblem;
            dlg.FileName = "[" + DateTime.Now.ToString("yyyy-MM-d_hh-mm-ss") + "] " + problem.Name + ".tex";
            dlg.DefaultExt = ".tex";
            dlg.Filter = "LaTeX files (.tex)|*.tex";  
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ProblemExporter exporter = new ProblemExporter();
                var problemItem = _ProblemManager.ProblemFullInfoList.First(x => x.Problem == problem);
                try
                {
                    exporter.SaveToTex(problemItem, dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Process.Start(dlg.FileName);
            }
        }
    }
}
