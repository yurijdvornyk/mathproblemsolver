using Microsoft.Win32;
using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.Manager;
using ProblemSolverApp.Classes.Manager.EventManager;
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
    /// Interaction logic for WorkspaceFiles.xaml
    /// </summary>
    public partial class WorkspaceFilesWindow : Window, IEventListener
    {
        public WorkspaceFilesWindow()
        {
            InitializeComponent();
            UpdateWorkspace();
        }

        #region Problem files

        public ObservableCollection<string> ProblemFiles
        {
            get { return (ObservableCollection<string>)GetValue(ProblemFilesProperty); }
            set { SetValue(ProblemFilesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProblemFiles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProblemFilesProperty =
            DependencyProperty.Register("ProblemFiles", typeof(ObservableCollection<string>), typeof(WorkspaceFilesWindow), new PropertyMetadata(new ObservableCollection<string>()));

        #endregion

        #region Library files

        public ObservableCollection<string> LibraryFiles
        {
            get { return (ObservableCollection<string>)GetValue(LibraryFilesProperty); }
            set { SetValue(LibraryFilesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LibraryFiles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LibraryFilesProperty =
            DependencyProperty.Register("LibraryFiles", typeof(ObservableCollection<string>), typeof(WorkspaceFilesWindow), new PropertyMetadata(new ObservableCollection<string>()));

        #endregion

        public Workspace CurrentWorkspace
        {
            get { return SessionManager.GetSession().CurrentWorkspace; }
        }

        public void UpdateWorkspace()
        {
            ProblemFiles.Clear();
            LibraryFiles.Clear();
            if (CurrentWorkspace == null)
            {
                setWindowEnabled(false);
            }
            else
            {
                setWindowEnabled(true);
                foreach (var problem in CurrentWorkspace.ProblemFiles)
                {
                    ProblemFiles.Add(problem);
                }
                foreach (var library in CurrentWorkspace.LibraryFiles)
                {
                    LibraryFiles.Add(library);
                }
            }
        }

        private void setWindowEnabled(bool enabled)
        {
            lbProblems.IsEnabled = enabled;
            lbLibraries.IsEnabled = enabled;
            btnAddLibraryFile.IsEnabled = enabled;
            btnAddProblemFile.IsEnabled = enabled;
            btnRemoveLibraryFile.IsEnabled = enabled;
            btnRemoveProblemFile.IsEnabled = enabled;
        }

        private void saveAndOpenWorkspace()
        {
            SessionManager.GetSession().SaveWorkspace(CurrentWorkspace.WorkspacePath);
            SessionManager.GetSession().OpenWorkspace(CurrentWorkspace.WorkspacePath);
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
                    saveAndOpenWorkspace();
                    UpdateWorkspace();
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
                saveAndOpenWorkspace();
                UpdateWorkspace();
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
                    saveAndOpenWorkspace();
                    UpdateWorkspace();
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
                saveAndOpenWorkspace();
                UpdateWorkspace();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void HandleEvent(EventType eventType, params object[] args)
        {
            switch (eventType)
            {
                case EventType.UpdateWorkspace:
                    UpdateWorkspace();
                    break;
                default:
                    break;
            }
        }
    }
}
