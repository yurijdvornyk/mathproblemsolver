﻿using System;
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

        public void CopySharedLibraries(bool allowReplaceFiles = false)
        {
            if (!Directory.Exists(Constants.APP_DATA_FOLDER_LIBS))
            {
                Directory.CreateDirectory(Constants.APP_DATA_FOLDER_LIBS);
            }
            foreach (var file in Directory.GetFiles(Constants.APP_DATA_FOLDER_LIBS))
            {
                if (Path.GetExtension(file).ToLower() == "." + Constants.DLL_EXTENSION_WITH_DOT)
                {
                    File.Copy(file, Path.Combine(Constants.CURRENT_DIRECTORY, Path.GetFileName(file)), allowReplaceFiles);
                }
            }
        }

        public void LoadSharedLibraries()
        {
            var libFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            List<LibraryItem> loadedContent = new List<LibraryItem>();
            foreach (var file in libFiles)
            {
                var asm = Assembly.LoadFrom(file); // Assembly.Load(AssemblyName.GetAssemblyName(file));
                loadedContent.Add(new LibraryItem(null, asm, Path.GetFileName(file)));
            }
            SharedLibraries = new ObservableCollection<LibraryItem>(loadedContent);
        }

        public void AddSharedLibraries(string[] files)
        {
            foreach (var file in files)
            {
                // TODO: Notify if file with the same name already exists
                string newFilePath = Path.Combine(Constants.APP_DATA_FOLDER_LIBS, "libs", Path.GetFileName(file));
                File.Copy(file, newFilePath);
            }
        }

        public void RemoveSharedLibraries()
        {
            foreach (var library in SharedLibraries)
            {
                library.Assembly = null;
            }

            foreach (var file in Directory.GetFiles(Constants.CURRENT_DIRECTORY))
            {
                if (Path.GetExtension(file).ToLower() == "." + Constants.DLL_EXTENSION_WITH_DOT)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Handle (Log)
                    }
                }
            }
        }

        #endregion
    }
}