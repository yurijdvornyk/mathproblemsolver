using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProblemSolverApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SessionManager.GetSession().RemoveSharedLibraries();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SessionManager.GetSession().CopySharedLibraries(true);
        }
    }
}
