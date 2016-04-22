using Microsoft.Win32;
using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.Session;
using ProblemSolverApp.Classes.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for WorkspaceEditorWindow.xaml
    /// </summary>
    public partial class WorkspaceEditorWindow : Window
    {
        public Workspace CurrentWorkspace { get; set; }

        public WorkspaceEditorWindow()
        {
            InitializeComponent();
            CurrentWorkspace = SessionManager.GetSession().CurrentWorkspace;

            if (CurrentWorkspace == null)
            {
                CurrentWorkspace = new Workspace();
            }

            displayWorkspaceValues();
        }

        private void displayWorkspaceValues()
        {
            tbWorkspacePath.Text = CurrentWorkspace.WorkspacePath;
            tbName.Text = CurrentWorkspace.Name;
            tbDescription.Text = StringUtils.HtmlToString(CurrentWorkspace.Description);

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

        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Contains("&") || e.Text.Contains("<") || e.Text.Contains(">") || e.Text.Contains(@"\") || e.Text.Contains("'"))
            {
                e.Handled = true;
            }
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!StringUtils.IsTextValidName(tbName.Text))
            {
                tbName.Text = tbName.Text.Replace("&", "").Replace("<", "").Replace(">", "").Replace(@"\", "").Replace("'", "");
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
            var problem = lbProblems.SelectedItem;
            if (problem != null)
            {
                string filename = problem.ToString();
                lbProblems.Items.Remove(problem);

                var problemFile = CurrentWorkspace.ProblemFiles.First(x => System.IO.Path.GetFileName(x) == filename);
                CurrentWorkspace.RemoveProblem(problemFile);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemoveSharedLibrary_Click(object sender, RoutedEventArgs e)
        {
            var library = lbSharedLibraries.SelectedItem;
            if (library != null)
            {
                string filename = library.ToString();
                lbSharedLibraries.Items.Remove(library);

                var libraryFile = CurrentWorkspace.LibraryFiles.First(x => System.IO.Path.GetFileName(x) == filename);
                CurrentWorkspace.RemoveLibrary(libraryFile);
            }
        }
    }
}
