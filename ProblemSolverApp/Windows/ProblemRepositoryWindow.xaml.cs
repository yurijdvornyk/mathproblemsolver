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
        }

        public SharedLibrariesManager SharedLibraries
        {
            get { return (SharedLibrariesManager)GetValue(_ProblemManagerProperty); }
            set { SetValue(_ProblemManagerProperty, value); }
        }

        public LibraryItem SelectedProblemLibrary { get; set; }

        public CustomLogger Logger { get; private set; }

        public static readonly DependencyProperty _ProblemManagerProperty =
            DependencyProperty.Register("_ProblemManager", typeof(SharedLibrariesManager), typeof(Window), new PropertyMetadata(null));

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
                    //SharedLibraries.AddLibrary(filename);

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

        private void findLibTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ObservableCollection<LibraryItem> libraries = new ObservableCollection<LibraryItem>();
            //var text = findLibTextBox.Text.ToLower();
            //if (!string.IsNullOrEmpty(text))
            //{
            //    foreach (var p in SharedLibraries.SharedLibrariesList)
            //    {
            //        if (p._AssemblyName != null)
            //        {
            //            if (p._AssemblyName.FullName.ToLower().Contains(text))
            //            {
            //                libraries.Add(p);
            //            }
            //        }
            //    }
            //    lbLibraries.ItemsSource = libraries;
            //}
            //else
            //{
            //    lbProblems.ItemsSource = SharedLibraries.SharedLibrariesList;
            //}
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //string result = "";
            //foreach (var i in SharedLibraries.ProblemFullInfoList)
            //{
            //    var modules = i._Assembly.GetReferencedAssemblies();
            //    result += i.Problem.Name + "\n";
            //    foreach (var j in modules)
            //    {
            //        result += "  " + j.Name + "\n";
            //    }
            //}
            //MessageBox.Show(result);
        }
    }
}