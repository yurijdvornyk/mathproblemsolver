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

        public bool doCopyFilesWhenPathChanged
        {
            get { return (bool)GetValue(doCopyFilesWhenPathChangedProperty); }
            set { SetValue(doCopyFilesWhenPathChangedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for doCopyFilesWhenPathChanged.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty doCopyFilesWhenPathChangedProperty =
            DependencyProperty.Register("doCopyFilesWhenPathChanged", typeof(bool), typeof(WorkspaceEditorWindow), new PropertyMetadata(false));

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
            tbName.Text = HttpUtility.HtmlDecode(CurrentWorkspace.Name);
            tbDescription.Text = HttpUtility.HtmlDecode(CurrentWorkspace.Description);
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

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            saveChanges();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (saveChanges())
            {
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
            string oldPath = CurrentWorkspace.WorkspacePath;
            string oldDirectory = System.IO.Path.GetDirectoryName(oldPath);
            CurrentWorkspace.WorkspacePath = tbWorkspacePath.Text;
            CurrentWorkspace.Name = HttpUtility.HtmlEncode(tbName.Text);
            CurrentWorkspace.Description = HttpUtility.HtmlEncode(tbDescription.Text);

            Workspace.Save(CurrentWorkspace.WorkspacePath, CurrentWorkspace);
            string newDirectory = System.IO.Path.GetDirectoryName(CurrentWorkspace.WorkspacePath);
            if (!string.IsNullOrEmpty(oldDirectory) && oldDirectory != newDirectory)
            {
                if (doCopyFilesWhenPathChanged)
                {
                    MessageBox.Show("You have changed the workspace path.\nOption to copy the workspace content is enabled.\nContent will be copied to new directory");
                }
                else
                {
                    MessageBox.Show("You have changed the workspace path.\nOption to copy the workspace content is disabled.\nContent will be moved to new directory");
                }

                foreach (var subfolder in Workspace.SUBFOLDERS)
                {
                    string oldSubfolder = System.IO.Path.Combine(oldDirectory, subfolder);
                    string newSubfolder = System.IO.Path.Combine(newDirectory, subfolder);
                    if (Directory.Exists(oldSubfolder))
                    {
                        if (doCopyFilesWhenPathChanged)
                        {
                            Directory.CreateDirectory(newSubfolder);
                            foreach (var file in Directory.GetFiles(oldSubfolder))
                            {
                                File.Copy(file, System.IO.Path.Combine(newSubfolder, System.IO.Path.GetFileName(file)));
                            }
                        }
                        else
                        {
                            Directory.Move(oldSubfolder, newSubfolder);
                            try
                            {
                                File.Delete(oldPath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
            AppEventManager.NotifyListeners(EventType.UpdateWorkspace);
            return true;
        }
    }
}
