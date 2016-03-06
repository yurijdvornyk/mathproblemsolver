using ProblemLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ProblemSolverApp.Classes
{
    public class ProblemManager
    {
        public string RepositoryPath { get; set; }
        public string ProblemPath { get { return RepositoryPath + pathSeparator + "problems"; } }
        public string SharedLibPath { get { return RepositoryPath + pathSeparator + "shared"; } }
        private string pathSeparator { get { return Path.DirectorySeparatorChar.ToString(); } }

        public ObservableCollection<ProblemItem> ProblemFullInfoList { get; set; }

        public ObservableCollection<LibraryItem> SharedLibrariesList { get; set; }

        private static ProblemManager problemManager;

        private ProblemManager()
        {
            //ProblemList = new ObservableCollection<IProblem>();
            ProblemFullInfoList = new ObservableCollection<ProblemItem>();
            SharedLibrariesList = new ObservableCollection<LibraryItem>();
        }

        public static ProblemManager GetInstance()
        {
            if (problemManager == null)
            {
                problemManager = new ProblemManager();
            }
            return problemManager;
        }

        public void LoadSharedLibraries(List<Assembly> builtInLibs)
        {
            string path = RepositoryPath + pathSeparator + "shared";
            var libFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
            //var libFiles = Directory.GetFiles(SharedLibPath);

            //List<LibraryItem> loadedContent = (from file in libFiles
            //                                   let asm = Assembly.LoadFrom(file)
            //                                   //from type in asm.GetTypes()
            //                                   //where typeof(object).IsAssignableFrom(type)
            //                                   where !builtInLibs.Contains(asm.FullName)
            //                                   select new LibraryItem(null, asm)).ToList();
            List<LibraryItem> loadedContent = new List<LibraryItem>();
            foreach (var file in libFiles)
            {
                var asm = Assembly.LoadFrom(file);
                if (!builtInLibs.Contains(asm))
                {
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
            }

            SharedLibrariesList = new ObservableCollection<LibraryItem>(loadedContent);
        }

        public void LoadProblems()
        {
            try
            {
                var problemsFiles = Directory.GetFiles(ProblemPath, "*.dll");

                List<ProblemItem> loadedContent = (from file in problemsFiles
                                    let asm = Assembly.LoadFrom(file)
                                    from type in asm.GetTypes()
                                    where typeof(IProblem).IsAssignableFrom(type)
                                    select new ProblemItem((IProblem)Activator.CreateInstance(type), asm)).ToList();

                ProblemFullInfoList = new ObservableCollection<ProblemItem>(loadedContent);
                //ProblemList = new ObservableCollection<IProblem>();
                //foreach (var p in ProblemFullInfoList)
                //{
                //    ProblemList.Add(p.Problem);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CopySharedLibraries(bool replace = true)
        {
            var destination = Directory.GetCurrentDirectory();
            var libFiles = Directory.GetFiles(SharedLibPath, "*.dll");
            foreach (var file in libFiles)
            {
                string newFileName = destination + pathSeparator + Path.GetFileName(file);
                if (!File.Exists(file))
                {
                    File.Copy(file, newFileName);
                }
                else
                {
                    try
                    {
                        File.Delete(newFileName);
                        File.Copy(file, newFileName);
                    }
                    catch (Exception ex)
                    {
                        //_Terminal.LogWarning("Can't update file '" + Path.GetFileName(newFileName) + 
                        //    "', because access is denied. Details:\n" + ex.Message);
                    }
                }
            }
        }

        public void Load(List<Assembly> builtInLibs, bool replaceLibsIfExist = true)
        {
            LoadSharedLibraries(builtInLibs);
            CopySharedLibraries(replaceLibsIfExist);
            LoadProblems();
        }

        public void AddProblem(params string[] fileFullNames)
        {
            List<Exception> exceptions = new List<Exception>();
            foreach (var fileFullName in fileFullNames)
            {
                try
                {
                    var asm = Assembly.LoadFrom(fileFullName);
                    var x = from type in asm.GetTypes()
                            where typeof(IProblem).IsAssignableFrom(type)
                            select type;
                    if (x.Count() == 0)
                    {
                        exceptions.Add(new ArgumentException("Problem you try to load is not valid. It should implement IProblem interface."));
                        continue;
                    }

                    string filename = Path.GetFileNameWithoutExtension(fileFullName);
                    string newFileFullName = ProblemPath + pathSeparator + Path.GetFileName(fileFullName);

                    try
                    {
                        File.Copy(fileFullName, newFileFullName);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        continue;
                    }

                    // We should update assembly file
                    asm = Assembly.LoadFrom(newFileFullName);
                    var problemInfoItem = new ProblemItem(
                        (IProblem)Activator.CreateInstance(x.First(y => typeof(IProblem).IsAssignableFrom(y))),
                        asm);
                    ProblemFullInfoList.Add(problemInfoItem);
                    //ProblemList.Add(problemInfoItem.Problem);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    continue;
                }
            }

            string message = string.Empty;
            if (exceptions.Count > 0)
            {
                foreach (var ex in exceptions)
                {
                    message += ex.Message + "\n";
                }
                throw new Exception(message);
            }
        }
        
        public void AddLibrary(params string[] originalFiles)
        {
            List<Exception> exceptions = new List<Exception>();
            foreach (var originalFile in originalFiles)
            {
                try
                {
                    string filename = Path.GetFileName(originalFile);
                    string newFileFullName = SharedLibPath + pathSeparator + filename;

                    if (File.Exists(newFileFullName))
                    {
                        exceptions.Add(new ArgumentException("File already exists: " + newFileFullName));
                    }

                    File.Copy(originalFile, newFileFullName);

                    try
                    {
                        File.Copy(originalFile, Directory.GetCurrentDirectory() + pathSeparator + filename);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(new UnauthorizedAccessException("Can't update file because access is denied. Details:\n" + ex.Message));
                    }

                    var asm = Assembly.LoadFrom(newFileFullName);
                    var lib = new LibraryItem(asm.GetName());
                    if (!SharedLibrariesList.Contains(lib))
                    {
                        SharedLibrariesList.Add(lib);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    continue;
                }
            }

            string message = string.Empty;
            if (exceptions.Count > 0)
            {
                foreach (var ex in exceptions)
                {
                    message += ex.Message + "\n";
                }
                throw new Exception(message);
            }
        }

        public void RemoveProblem(string name)
        {
        }

        public void Calculate(int index)
        {
            var problem = ProblemFullInfoList[index].Problem;
            try
            {
                
                if (problem.IsInputDataSet)
                {
                    problem.Solve();
                    //_Terminal.LogSuccess("Problem '" + problem.Name + "' calculated successfully.");
                }
            }
            catch (Exception ex)
            {
                //_Terminal.LogError("There were some errors while calculating the problem '" + problem.Name + "'. Details:\n" + ex.Message);
            }
        }

        public void Calculate(IProblem problem)
        {
            int index = ProblemFullInfoList.IndexOf(ProblemFullInfoList.First(x => x.Problem == problem));
            Calculate(index);
        }

        public IProblem GetProblem(IProblem problem)
        {
            int index = ProblemFullInfoList.IndexOf(ProblemFullInfoList.First(x => x.Problem == problem));
            return GetProblem(index);
        }

        public IProblem GetProblem(int index)
        {
            try
            {
                return ProblemFullInfoList[index].Problem;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Can't find problem. Details:\n" + ex.Message);
            }
        }
    }
}