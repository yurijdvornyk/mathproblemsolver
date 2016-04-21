using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.Session;
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
    /// Interaction logic for WorkspaceDescriptionWindow.xaml
    /// </summary>
    public partial class WorkspaceDescriptionWindow : Window
    {
        public WorkspaceDescriptionWindow()
        {
            InitializeComponent();
            Workspace workspace = SessionManager.GetSession().CurrentWorkspace;
            if (workspace != null)
            {
                webBrowser.NavigateToString("<h1>" + workspace.Name + "</h1><br/>" + workspace.Description);
            }
            else
            {
                webBrowser.NavigateToString("Workspace is not opened.");
            }
        }
    }
}
