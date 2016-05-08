using Microsoft.Win32;
using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.Manager;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for WorkspaceFiles.xaml
    /// </summary>
    public partial class WorkspaceFilesWindow : Window
    {
        public WorkspaceFilesWindow()
        {
            InitializeComponent();
            UpdateWorkspace();
        }

        public Workspace CurrentWorkspace
        {
            get { return (Workspace)GetValue(CurrentWorkspaceProperty); }
            set { SetValue(CurrentWorkspaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentWorkspace.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentWorkspaceProperty =
            DependencyProperty.Register("CurrentWorkspace", typeof(Workspace), typeof(WorkspaceFilesWindow), new PropertyMetadata(null));

        public void UpdateWorkspace()
        {
            CurrentWorkspace = SessionManager.GetSession().CurrentWorkspace;
        }

        private void btnAddProblemFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Dynamic-link library (.dll)|*.dll|All files (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CurrentWorkspace.CopyToDirectory(Workspace.PROBLEMS_PATH, dialog.FileNames);
                    List<string> filenames = new List<string>();
                    foreach (var file in dialog.FileNames)
                    {
                        filenames.Add(System.IO.Path.GetFileName(file));
                    }
                    CurrentWorkspace.AddProblems(filenames.ToArray());
                    string path = CurrentWorkspace.WorkspacePath;
                    SessionManager.GetSession().CloseWorkspace();
                    SessionManager.GetSession().OpenWorkspace(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemoveProblemFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> problems = new List<string>();
                foreach (var problem in lbProblems.SelectedItems)
                {
                    problems.Add(problem.ToString());
                }
                CurrentWorkspace.RemoveProblems(problems.ToArray());
                string path = CurrentWorkspace.WorkspacePath;
                SessionManager.GetSession().CloseWorkspace();
                SessionManager.GetSession().OpenWorkspace(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddLibraryFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Dynamic-link library (.dll)|*.dll|All files (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CurrentWorkspace.CopyToDirectory(Workspace.LIBRARIES_PATH, dialog.FileNames);
                    List<string> filenames = new List<string>();
                    foreach (var file in dialog.FileNames)
                    {
                        filenames.Add(System.IO.Path.GetFileName(file));
                    }
                    CurrentWorkspace.AddLibraries(filenames.ToArray());
                    string path = CurrentWorkspace.WorkspacePath;
                    SessionManager.GetSession().CloseWorkspace();
                    SessionManager.GetSession().OpenWorkspace(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemoveLibraryFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> libraries = new List<string>();
                foreach (var library in lbLibraries.SelectedItems)
                {
                    libraries.Add(library.ToString());
                }
                CurrentWorkspace.RemoveLibraries(libraries.ToArray());
                string path = CurrentWorkspace.WorkspacePath;
                SessionManager.GetSession().CloseWorkspace();
                SessionManager.GetSession().OpenWorkspace(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
