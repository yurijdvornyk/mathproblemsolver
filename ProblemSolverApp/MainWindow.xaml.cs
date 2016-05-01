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
using ProblemDevelopmentKit;
using ProblemSolverApp.Windows;
using ProblemSolverApp.Classes;
using System.Reflection;
using System.Globalization;
using ProblemSolverApp.Classes.CustomLogger;
using Microsoft.Win32;
using System.Diagnostics;
using ProblemSolverApp.Classes.Manager;
using ProblemSolverApp.Classes.Manager.EventManager;

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
            Session = SessionManager.GetSession();
            Session.LoadSharedLibraries();
            AppEventManager.AddListener(problemDataControl);
        }

        public SessionManager Session { get; private set; } 

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var problem = problemDataControl.CurrentProblem;
            if (problem == null)
            {
                MessageBox.Show("To calculate problem, select it first", "Problem not selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string name = string.Empty;
            try
            {
                name = problem.Name;
                problem.Solve();
                problemResults.CurrentProblem = problem;
                problemResults.UpdateResults();
                //Logger.LogSuccess(problem.Name + ": Problem calculated successfully.");
            }
            catch (Exception ex)
            {
                // TODO: improve
                MessageBox.Show(ex.Message);
                //Logger.LogError(name + ": There were some errors while calculating the problem. Details:\n" + ex.Message);
            }
        }

        private void btnProblemManager_Click(object sender, RoutedEventArgs e)
        {
            SharedLibrariesRepositoryWindow pmWin = new SharedLibrariesRepositoryWindow();
            //Logger.LogInfo("Open problem repository.");
            pmWin.Show();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWin = new SettingsWindow();
            //Logger.LogInfo("Open application settings.");
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
                var problemItem = Session.CurrentWorkspace.GetProblem(problem);
                try
                {
                    // TODO: add
                    //exporter.SaveToTex(problemItem, dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Process.Start(dlg.FileName);
            }
        }

        private void btnOpenWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".workspace";
            dialog.Filter = "MathProblemSolver workspace (.workspace)|*.workspace";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Session.OpenWorkspace(dialog.FileName);
                    problemDataControl.ReloadWorkspace();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnNewWorkspace_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceEditorWindow window = new WorkspaceEditorWindow();
            window.Show();
        }

        private void btnCloseWorkspace_Click(object sender, RoutedEventArgs e)
        {
            Session.CloseWorkspace();
        }
    }
}
