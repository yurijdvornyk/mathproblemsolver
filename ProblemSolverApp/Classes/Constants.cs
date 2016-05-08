using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes
{
    public class Constants
    {
        public static readonly string PATH_SEPARATOR = Path.DirectorySeparatorChar.ToString();
        public static readonly string CURRENT_DIRECTORY = Directory.GetCurrentDirectory();
        public static readonly string DLL_EXTENSION_WITH_DOT = ".dll";
        public static readonly string APP_DATA_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MathProblemSolver");
        public static readonly string APP_DATA_FOLDER_LIBS = Path.Combine(APP_DATA_FOLDER, "libs");

        // Write all libraries that must not be removed on app stsrtup here!
        public static string[] APP_BUILT_IN_DLLS = { "ProblemDevelopmentKit.dll", "SpotLibrary.dll" };
    }
}