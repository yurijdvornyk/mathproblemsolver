using Microsoft.Win32;
using ProblemLibrary;
using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.CustomLogger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace ProblemSolverApp.Windows
{
    /// <summary>
    /// Interaction logic for ProblemRepositoryWindow.xaml
    /// </summary>
    public partial class ProblemRepositoryWindow : Window
    {
        public ProblemRepositoryWindow()
        {
            InitializeComponent();
        }

        public ProblemRepositoryWindow(CustomLogger logger, string path)
        {
            InitializeComponent();
            Logger = logger;

            _ProblemManager = ProblemManager.GetInstance();
            _ProblemManager.RepositoryPath = path;
        }

        public ProblemManager _ProblemManager
        {
            get { return (ProblemManager)GetValue(_ProblemManagerProperty); }
            set { SetValue(_ProblemManagerProperty, value); }
        }

        public LibraryItem SelectedProblemLibrary { get; set; }

        public CustomLogger Logger { get; private set; }

        public static readonly DependencyProperty _ProblemManagerProperty =
            DependencyProperty.Register("_ProblemManager", typeof(ProblemManager), typeof(Window), new PropertyMetadata(null));

        private void btnAddProblem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.DefaultExt = ".dll";
            dlg.Filter = "Problem library (.dll)|*.dll";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                var filenames = dlg.FileNames;
                try
                {
                    _ProblemManager.AddProblem(filenames);

                    string message = "Problem(s) loaded successfully.";
                    Logger.LogSuccess(message);
                }
                catch (Exception ex)
                {
                    string message = "Cannot load problem(s). Check if file(s) for errors. Details:\n" + ex.Message;
                    Logger.LogError(message);
                }
                MessageBox.Show("Loading process finished. Check logs for more details.", "Loading process finished.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnLoadProblemsLib_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".dll";
            dlg.Filter = "Dynamic-link library (.dll)|*.dll";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                try
                {
                    _ProblemManager.AddLibrary(filename);
                    Logger.LogSuccess("Library from file " + filename + " loaded successfully.");
                    MessageBox.Show("Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Logger.LogError("Cannot load library: " + filename + ". Check if the file is valid. Details:\n" + ex.Message);
                }
            }
        }

        private void btnAddLibrary_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".dll";
            dlg.Multiselect = true;
            dlg.Filter = "Dynamic-link library (.dll)|*.dll";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                try
                {
                    _ProblemManager.AddLibrary(filename);

                    string message = "Library from file " + filename + " loaded successfully.";
                    Logger.LogSuccess(message);
                    MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Logger.LogWarning(ex.Message);
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    string message = "Cannot load problem: " + filename + ". Check if the file is valid. Details:\n" + ex.Message;
                    Logger.LogError(message);
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnRemoveLibrary_Click(object sender, RoutedEventArgs e)
        {
        }

        private void findProblemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<ProblemItem> problems = new ObservableCollection<ProblemItem>();
            var text = findProblemTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var p in _ProblemManager.ProblemFullInfoList)
                {
                    if (p.Problem.Name.ToLower().Contains(text))
                    {
                        problems.Add(p);
                    }
                }
                lbProblems.ItemsSource = problems;
            }
            else
            {
                lbProblems.ItemsSource = _ProblemManager.ProblemFullInfoList;
            }
        }

        private void findLibTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<LibraryItem> libraries = new ObservableCollection<LibraryItem>();
            var text = findLibTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var p in _ProblemManager.SharedLibrariesList)
                {
                    if (p._AssemblyName != null)
                    {
                        if (p._AssemblyName.FullName.ToLower().Contains(text))
                        {
                            libraries.Add(p);
                        }
                    }
                }
                lbLibraries.ItemsSource = libraries;
            }
            else
            {
                lbProblems.ItemsSource = _ProblemManager.SharedLibrariesList;
            }
        }
    }
}