using Microsoft.Win32;
using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.Manager;
using ProblemSolverApp.Classes.Manager.EventManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
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
    /// Interaction logic for WorkspaceEditorWindow.xaml
    /// </summary>
    public partial class WorkspaceEditorWindow : Window
    {
        public Workspace CurrentWorkspace { get; set; }
        private bool needsRestart;

        public WorkspaceEditorWindow()
        {
            InitializeComponent();
            CurrentWorkspace = SessionManager.GetSession().CurrentWorkspace;
            needsRestart = false;

            if (CurrentWorkspace == null)
            {
                CurrentWorkspace = new Workspace();
            }

            displayWorkspaceValues();
        }

        private void displayWorkspaceValues()
        {
            tbWorkspacePath.Text = CurrentWorkspace.WorkspacePath;
            tbName.Text = HttpUtility.HtmlDecode(CurrentWorkspace.Name);
            tbDescription.Text = HttpUtility.HtmlDecode(CurrentWorkspace.Description);

            updateProblemsList();
            updateSharedLibrariesList();
        }

        private void updateProblemsList()
        {
            lbProblems.Items.Clear();
            var sortedList = new SortedSet<string>(CurrentWorkspace.ProblemFiles);
            foreach (var i in sortedList)
            {
                lbProblems.Items.Add(System.IO.Path.GetFileName(i));
            }
        }

        private void updateSharedLibrariesList()
        {
            lbSharedLibraries.Items.Clear();
            var sortedList = new SortedSet<string>(CurrentWorkspace.LibraryFiles);
            foreach (var i in sortedList)
            {
                lbSharedLibraries.Items.Add(System.IO.Path.GetFileName(i));
            }
        }

        private void btnSetWorkspacePath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".workspace";
            dialog.Filter = "MathProblemSolver workspace (.workspace)|*.workspace";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    tbWorkspacePath.Text = dialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAddProblem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Dynamic-link library (.dll)|*.dll|All files (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CurrentWorkspace.AddProblems(dialog.FileNames);
                    updateProblemsList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemoveProblem_Click(object sender, RoutedEventArgs e)
        {
            if (lbProblems.SelectedItems.Count > 0)
            {
                foreach (var problem in lbProblems.SelectedItems)
                {
                    string filename = problem.ToString();
                    var problemFile = CurrentWorkspace.ProblemFiles.First(x => System.IO.Path.GetFileName(x) == filename);
                    CurrentWorkspace.RemoveProblem(problemFile);
                }
                updateProblemsList();
            }
        }

        private void btnAddSharedLibrary_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Dynamic-link library (.dll)|*.dll|All files (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CurrentWorkspace.AddLibraries(dialog.FileNames);
                    updateProblemsList();
                    needsRestart = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemoveSharedLibrary_Click(object sender, RoutedEventArgs e)
        {
            foreach (var library in lbSharedLibraries.SelectedItems)
            {
                string filename = library.ToString();
                lbSharedLibraries.Items.Remove(library);

                var libraryFile = CurrentWorkspace.LibraryFiles.First(x => System.IO.Path.GetFileName(x) == filename);
                CurrentWorkspace.RemoveLibrary(libraryFile);                
            }
            needsRestart = true;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (saveChanges())
            {
                if (needsRestart)
                {
                    // Restart app
                }

                Close();
            }
        }

        private bool saveChanges()
        {
            if (string.IsNullOrEmpty(tbWorkspacePath.Text))
            {
                MessageBox.Show("Workspace path must be set!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("Workspace name must be set!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            CurrentWorkspace.WorkspacePath = tbWorkspacePath.Text;
            CurrentWorkspace.Name = HttpUtility.HtmlEncode(tbName.Text);
            CurrentWorkspace.Description = HttpUtility.HtmlEncode(tbDescription.Text);

            CurrentWorkspace.ProblemFiles.Clear();
            foreach (var item in lbProblems.Items)
            {
                CurrentWorkspace.ProblemFiles.Add(item.ToString());
            }
            CurrentWorkspace.LoadAllProblems();

            CurrentWorkspace.LibraryFiles.Clear();
            foreach (var item in lbSharedLibraries.Items)
            {
                CurrentWorkspace.LibraryFiles.Add(item.ToString());
            }

            if (!File.Exists(CurrentWorkspace.WorkspacePath))
            {
                try
                {
                    Workspace.Save(CurrentWorkspace.WorkspacePath, CurrentWorkspace);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (needsRestart)
            {
                MessageBox.Show("The application will be restarted to have problems and shared libraries lists updated.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            AppEventManager.NotifyListeners(EventType.UpdateWorkspace);
            return true;
        }
    }
}
