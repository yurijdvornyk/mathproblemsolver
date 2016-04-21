using ProblemLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ProblemSolverApp.Classes
{
    public class SharedLibrariesManager
    {
        //public string RepositoryPath { get; set; }

        //public ObservableCollection<LibraryItem> SharedLibrariesList { get; set; }

        //private static SharedLibrariesManager libraryManager;

        //private SharedLibrariesManager()
        //{
        //    SharedLibrariesList = new ObservableCollection<LibraryItem>();
        //}

        //public static SharedLibrariesManager GetInstance()
        //{
        //    if (libraryManager == null)
        //    {
        //        libraryManager = new SharedLibrariesManager();
        //    }
        //    return libraryManager;
        //}

        //public void LoadSharedLibraries(List<Assembly> builtInLibs)
        //{
        //    string path = RepositoryPath + pathSeparator + "shared";
        //    var libFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
        //    //var libFiles = Directory.GetFiles(SharedLibPath);

        //    //List<LibraryItem> loadedContent = (from file in libFiles
        //    //                                   let asm = Assembly.LoadFrom(file)
        //    //                                   //from type in asm.GetTypes()
        //    //                                   //where typeof(object).IsAssignableFrom(type)
        //    //                                   where !builtInLibs.Contains(asm.FullName)
        //    //                                   select new LibraryItem(null, asm)).ToList();
        //    List<LibraryItem> loadedContent = new List<LibraryItem>();
        //    foreach (var file in libFiles)
        //    {
        //        var asm = Assembly.LoadFrom(file);
        //        if (!builtInLibs.Contains(asm))
        //        {
        //            var typeObjects = new List<object>();
        //            var types = asm.GetTypes();
        //            foreach (var type in types)
        //            {
        //                try
        //                {
        //                    typeObjects.Add(Activator.CreateInstance(type));
        //                }
        //                catch
        //                {
        //                    typeObjects.Add(null);
        //                }
        //            }
        //            loadedContent.Add(new LibraryItem(null, asm, typeObjects));
        //        }
        //    }

        //    SharedLibrariesList = new ObservableCollection<LibraryItem>(loadedContent);
        //}

        //public void CopySharedLibraries(bool replace = true)
        //{
        //    var destination = Directory.GetCurrentDirectory();
        //    var libFiles = Directory.GetFiles(SharedLibPath, "*.dll");
        //    foreach (var file in libFiles)
        //    {
        //        string newFileName = destination + pathSeparator + Path.GetFileName(file);
        //        if (!File.Exists(file))
        //        {
        //            File.Copy(file, newFileName);
        //        }
        //        else
        //        {
        //            try
        //            {
        //                File.Delete(newFileName);
        //                File.Copy(file, newFileName);
        //            }
        //            catch (Exception ex)
        //            {
        //                //_Terminal.LogWarning("Can't update file '" + Path.GetFileName(newFileName) + 
        //                //    "', because access is denied. Details:\n" + ex.Message);
        //            }
        //        }
        //    }
        //}

        //public void Load(List<Assembly> builtInLibs, bool replaceLibsIfExist = true)
        //{
        //    LoadSharedLibraries(builtInLibs);
        //    CopySharedLibraries(replaceLibsIfExist);
        //    LoadProblems();
        //}
        
        //public void AddLibrary(params string[] originalFiles)
        //{
        //    List<Exception> exceptions = new List<Exception>();
        //    foreach (var originalFile in originalFiles)
        //    {
        //        try
        //        {
        //            string filename = Path.GetFileName(originalFile);
        //            string newFileFullName = SharedLibPath + pathSeparator + filename;

        //            if (File.Exists(newFileFullName))
        //            {
        //                exceptions.Add(new ArgumentException("File already exists: " + newFileFullName));
        //            }

        //            File.Copy(originalFile, newFileFullName);

        //            try
        //            {
        //                File.Copy(originalFile, Directory.GetCurrentDirectory() + pathSeparator + filename);
        //            }
        //            catch (Exception ex)
        //            {
        //                exceptions.Add(new UnauthorizedAccessException("Can't update file because access is denied. Details:\n" + ex.Message));
        //            }

        //            var asm = Assembly.LoadFrom(newFileFullName);
        //            var lib = new LibraryItem(asm.GetName());
        //            if (!SharedLibrariesList.Contains(lib))
        //            {
        //                SharedLibrariesList.Add(lib);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            exceptions.Add(ex);
        //            continue;
        //        }
        //    }

        //    string message = string.Empty;
        //    if (exceptions.Count > 0)
        //    {
        //        foreach (var ex in exceptions)
        //        {
        //            message += ex.Message + "\n";
        //        }
        //        throw new Exception(message);
        //    }
        //}        
    }
}