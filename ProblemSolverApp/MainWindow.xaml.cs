using System;
using System.Windows;
using ProblemSolverApp.Windows;
using ProblemSolverApp.Classes;
using Microsoft.Win32;
using System.Diagnostics;
using ProblemSolverApp.Classes.Manager;
using ProblemSolverApp.Classes.Manager.EventManager;
using System.Threading.Tasks;
using ProblemDevelopmentKit.Logger;
using ProblemDevelopmentKit.Progress;

namespace ProblemSolverApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISolutionProgressListener
    {
        public MainWindow()
        {
            InitializeComponent();
            Session = SessionManager.GetSession();
            Session.LoadSharedLibraries();
            setUpListeners();
        }

        public SessionManager Session { get; private set; }

        private void setUpListeners()
        {
            AppEventManager.AddListener(problemDataControl);
            ProblemProgressNotifier.RegisterListener(this);
        }

        private async void btnCalculate_Click(object sender, RoutedEventArgs e)
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
                progressBar.IsIndeterminate = true;
                await Task.Run(() => Session.CurrentWorkspace.SolveProblem(problem));
                progressBar.IsIndeterminate = true;
                problemResults.CurrentProblem = problem;
                problemResults.UpdateResults();
                progressBar.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                // TODO: improve
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProblemManager_Click(object sender, RoutedEventArgs e)
        {
            SharedLibrariesRepositoryWindow pmWin = new SharedLibrariesRepositoryWindow();
            pmWin.Show();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWin = new SettingsWindow();
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

        #region ISolvingProgressListener imeplementation

        public int Filter { get; set; }

        public void SetProgressModeEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                Dispatcher.Invoke(() => progressBar.IsIndeterminate = false);
                
            }
            else
            {
                Dispatcher.Invoke(() => progressBar.IsIndeterminate = true);
            }
        }

        public void SetProgress(double percent)
        {
            Dispatcher.Invoke(() =>
            {
                if (!progressBar.IsIndeterminate)
                {
                    progressBar.Value = percent;
                }
            }
            );
        }

        #endregion
    }
}