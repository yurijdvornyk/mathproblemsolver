using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            foreach (var file in Directory.GetFiles(Constants.CURRENT_DIRECTORY))
            {
                if (Path.GetExtension(file).ToLower() == Constants.DLL_EXTENSION_WITH_DOT)
                {
                    if (!Constants.APP_BUILT_IN_DLLS.Contains(Path.GetFileName(file)))
                    {
                        File.Delete(file);
                    }
                }
            }
            SessionManager.GetSession().CopySharedLibraries(true);
        }
    }
}