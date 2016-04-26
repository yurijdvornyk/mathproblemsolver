using Microsoft.Win32;
using ProblemLibrary;
using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.CustomLogger;
using ProblemSolverApp.Classes.Manager;
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
    /// Interaction logic for SharedLibrariesRepositoryWindow.xaml
    /// </summary>
    public partial class SharedLibrariesRepositoryWindow : Window
    {
        public SharedLibrariesRepositoryWindow()
        {
            InitializeComponent();
            lvLibraries.ItemsSource = SessionManager.GetSession().SharedLibraries;
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
                string[] filenames = dlg.FileNames;
                try
                {
                    SessionManager.GetSession().AddSharedLibraries(filenames);

                    string message = string.Empty;
                    if (filenames.Length == 1)
                    {
                        message = "Library from file " + filenames + " loaded successfully.";
                    } else if (filenames.Length > 1)
                    {
                        message = filenames.Length + " libraries were loaded successfully.";
                    }
                    MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    //string message = "Cannot load problem: " + filenames + ". Check if the file is valid. Details:\n" + ex.Message;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnRemoveLibrary_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add
        }

        private void findLibTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = findLibTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(text))
            {
                lvLibraries.ItemsSource = SessionManager
                    .GetSession()
                    .SharedLibraries
                    .Where(x => x.AssemblyName != null && x.AssemblyName.FullName.ToLower().Contains(text));
            }
            else
            {
                lvLibraries.ItemsSource = SessionManager.GetSession().SharedLibraries;
            }
        }
    }
}