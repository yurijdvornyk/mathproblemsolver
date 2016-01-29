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
        public AssemblyName _AssemblyName { get; set; }
        public Assembly _Assembly { get; set; }
        public List<object> Instances { get; set; }

        public bool IsAssemblyLoaded
        {
            get
            {
                if (_Assembly == null && LibraryInstance == null) { return false; }
                return true;
            }
        }

        public LibraryItem(object instance, Assembly assembly, IEnumerable<object> instances = null)
        {
            LibraryInstance = instance;
            _Assembly = assembly;
            _AssemblyName = assembly.GetName();
            Instances = new List<object>();
            
            if (instances != null)
            {
                foreach (var i in instances)
                {
                    Instances.Add(i);
                }
            }
        }

        public LibraryItem(AssemblyName assemblyName, IEnumerable<object> instances = null)
        {
            _AssemblyName = assemblyName;
            Instances = new List<object>();

            if (instances != null)
            {
                foreach (var i in instances)
                {
                    Instances.Add(i);
                }
            }
        }

        //public override bool Equals(object obj)
        //{
        //    LibraryItem library = obj as LibraryItem;
        //    if (obj == null) { return false; }

        //    if (library._AssemblyName.FullName == this._AssemblyName.FullName)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public override string ToString()
        {
            return _AssemblyName.Name;
        }
    }
}