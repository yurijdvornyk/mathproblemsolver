using System;
using ProblemDevelopmentKit;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace ProblemSolverApp.Classes
{
    public class ProblemItem
    {
        public IProblem Problem { get; private set; }
        public BitmapImage EquationImage { get; private set; }

        public Assembly _Assembly { get; private set; }

        public string AssemblyName { get { return _Assembly.FullName; } }
        public string AssemblyVersion { get { return _Assembly.GetName().Version.Major + "." + _Assembly.GetName().Version.Minor; } }
        public List<LibraryItem> ReferencedAssemblies
        {
            get
            {
                List<LibraryItem> result = new List<LibraryItem>();
                foreach (var i in _Assembly.GetReferencedAssemblies())
                {
                    result.Add(new LibraryItem(i, ""));
                }
                return result;
            }
        }
        public bool AreDependenciesResolved
        {
            get
            {
                foreach (var i in ReferencedAssemblies)
                {
                    if (!i.IsAssemblyReferenced)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public ProblemItem(IProblem problem, Assembly assembly)
        {
            Problem = problem;
            _Assembly = assembly;

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var loadedAssemblyNames = new List<string>();
            foreach (var i in loadedAssemblies)
            {
                loadedAssemblyNames.Add(i.GetName().ToString());
            }

            try
            {
                string url = @"http://chart.apis.google.com/chart?cht=tx&chf=bg,s,AAAAAA00&chs=" + 60 + "&chl=" + problem.Equation.Replace("+", "%2B");
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                EquationImage = bitmap;
            }
            catch
            {
                EquationImage = null;
            }
        }

        public override string ToString()
        {
            return Problem.Name;
        }
    }
}