using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.Manager
{
    public class SessionManager
    {
        private static SessionManager session;

        public Workspace CurrentWorkspace { get; private set; }

        public ObservableCollection<LibraryItem> SharedLibraries { get; private set; }

        private SessionManager()
        {
            CurrentWorkspace = null;
            SharedLibraries = new ObservableCollection<LibraryItem>();
        }

        public static SessionManager GetSession()
        {
            if (session == null)
            {
                session = new SessionManager();
            }
            return session;
        }

        #region Work with Workspace

        public void OpenWorkspace(string filename)
        {
            CurrentWorkspace = Workspace.Load(filename);
        }

        public void NewWorkspace(string name, string description = "")
        {
            CurrentWorkspace = new Workspace(name, description);
        }

        public void SaveWorkspace(string filename)
        {
            Workspace.Save(filename, CurrentWorkspace);
        }

        public void CloseWorkspace()
        {
            Workspace.Close(CurrentWorkspace);
        }

        #endregion

        #region Work with shared libraries

        public void CopySharedLibraries()
        {
            string specificFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MathProblemSolver");
            if (!Directory.Exists(specificFolder))
            {
                Directory.CreateDirectory(specificFolder);
            }
            foreach (var file in Directory.GetFiles(specificFolder))
            {
                if (Path.GetExtension(file).ToLower() == "." + Constants.DLL_EXTENSION)
                {
                    File.Copy(file, Path.Combine(Constants.CURRENT_DIRECTORY, Path.GetFileName(file)), false);
                }
            }
        }

        public void LoadSharedLibraries()
        {
            var libFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            List<LibraryItem> loadedContent = new List<LibraryItem>();
            foreach (var file in libFiles)
            {
                var asm = Assembly.LoadFrom(file);
                var typeObjects = new List<object>();
                var types = asm.GetTypes();
                foreach (var type in types)
                {
                    try
                    {
                        typeObjects.Add(Activator.CreateInstance(type));
                    }
                    catch
                    {
                        typeObjects.Add(null);
                    }
                }
                loadedContent.Add(new LibraryItem(null, asm, typeObjects));
            }
            SharedLibraries = new ObservableCollection<LibraryItem>(loadedContent);
        }

        public void RemoveSharedLibraries()
        {
            foreach (var file in Directory.GetFiles(Constants.CURRENT_DIRECTORY))
            {
                if (Path.GetExtension(file).ToLower() == "." + Constants.DLL_EXTENSION)
                {
                    File.Delete(file);
                }
            }
        }

        #endregion
    }
}