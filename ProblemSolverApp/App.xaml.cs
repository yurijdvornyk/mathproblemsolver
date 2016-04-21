using ProblemSolverApp.Classes;
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
            string file = Constants.CURRENT_DIRECTORY + @"\HomericLibrary.dll";
            File.Delete(file);
        }
    }
}
