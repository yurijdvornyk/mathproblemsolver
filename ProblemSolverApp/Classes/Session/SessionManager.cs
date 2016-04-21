using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.Session
{
    public class SessionManager
    {
        private static SessionManager Session;

        public Workspace CurrentWorkspace { get; private set; }

        public string SessionPath { get; set; }

        public ObservableCollection<LibraryItem> SharedLibraries { get; private set; }

        private SessionManager()
        {
            CurrentWorkspace = null;
            SharedLibraries = new ObservableCollection<LibraryItem>();
        }

        public static SessionManager GetSession()
        {
            if (Session == null)
            {
                Session = new SessionManager();
            }
            return Session;
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
            string path = SessionPath + Constants.PATH_SEPARATOR + "libraries" + Constants.PATH_SEPARATOR;
            var libFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            foreach (var file in libFiles)
            {
                var newFile = path + Path.GetFileName(file);
                File.Copy(file, newFile, false);
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

        #endregion
    }
}