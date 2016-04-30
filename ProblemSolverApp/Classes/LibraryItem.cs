using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ProblemSolverApp.Classes
{
    public class LibraryItem
    {
        public object LibraryInstance { get; set; } 
        public AssemblyName AssemblyName { get; set; }
        public Assembly Assembly { get; set; }
        public string AssemblyFileName { get; private set; }

        public bool IsAssemblyLoaded
        {
            get
            {
                if (Assembly == null && LibraryInstance == null) { return false; }
                return true;
            }
        }

        public bool IsAssemblyReferenced
        {
            get
            {
                List<Assembly> assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
                if (assemblies.Exists(x => x.GetName().Name == AssemblyName.Name && x.GetName().Version == AssemblyName.Version))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public LibraryItem(object instance, Assembly assembly, string filename)
        {
            LibraryInstance = instance;
            Assembly = assembly;
            AssemblyName = assembly.GetName();
            AssemblyFileName = filename;
        }

        public LibraryItem(AssemblyName assemblyName, string filename)
        {
            AssemblyName = assemblyName;
            AssemblyFileName = filename;
        }

        public override string ToString()
        {
            return AssemblyName.FullName;
        }
    }
}