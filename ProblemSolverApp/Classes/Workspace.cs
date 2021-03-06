﻿using ProblemDevelopmentKit;
using ProblemDevelopmentKit.Logger;
using ProblemDevelopmentKit.Progress;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProblemSolverApp.Classes
{
    [Serializable]
    [XmlRoot(ElementName = "workspace")]
    public class Workspace
    {
        public static readonly string PROBLEMS_PATH = "problems";
        public static readonly string LIBRARIES_PATH = "libs";
        public static readonly string[] SUBFOLDERS = { PROBLEMS_PATH, LIBRARIES_PATH };

        [XmlElement(ElementName = "id")]
        public Guid Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlArray(ElementName = "problems")]
        [XmlArrayItem(ElementName = "problem")]
        public ObservableCollection<string> ProblemFiles { get; }

        [XmlArray(ElementName = "libraries")]
        [XmlArrayItem(ElementName = "library")]
        public ObservableCollection<string> LibraryFiles { get; }

        [XmlIgnore]
        public ObservableCollection<ProblemItem> Problems { get; private set; }

        [XmlIgnore]
        public ObservableCollection<LibraryItem> Libraries { get; set; }

        [XmlIgnore]
        public string WorkspacePath { get; set; }

        public Workspace() : this("", "", null, null) { }

        public Workspace(string name, string description) : this(name, description, null, null) { }

        public Workspace(string name, string description, List<string> problemFiles, List<string> libraryFiles)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;
            WorkspacePath = null;

            if (name != null)
            {
                Name = name;
            }
            if (description != null)
            {
                Description = description;
            }

            if (problemFiles != null)
            {
                ProblemFiles = new ObservableCollection<string>(problemFiles);
            }
            else
            {
                ProblemFiles = new ObservableCollection<string>();
            }

            if (libraryFiles != null)
            {
                LibraryFiles = new ObservableCollection<string>(libraryFiles);
            }
            else
            {
                LibraryFiles = new ObservableCollection<string>();
            }
        }

        public static Workspace Load(string filename)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Workspace));
                TextReader reader = new StreamReader(filename);
                Workspace workspace = (Workspace)xmlSerializer.Deserialize(reader);
                reader.Close();
                workspace.WorkspacePath = filename;
                workspace.LoadAllLibraries();
                workspace.LoadAllProblems();
                return workspace;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Save(string filename, Workspace workspace)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Workspace));
            TextWriter writer = new StreamWriter(filename);
            xmlSerializer.Serialize(writer, workspace);
            writer.Close();
        }

        public static void Close(Workspace workspace)
        {
            workspace = null;
        }

        public List<ProblemItem> LoadProblemFromFile(string problemFileName)
        {
            string problemsPath = Path.Combine(Path.GetDirectoryName(WorkspacePath), PROBLEMS_PATH, problemFileName);
            var asm = Assembly.LoadFrom(problemsPath);
            var result = new List<ProblemItem>();
            foreach (var type in asm.GetTypes())
            {
                if (typeof(IProblem).IsAssignableFrom(type))
                {
                    result.Add(new ProblemItem((IProblem)Activator.CreateInstance(type), asm));
                }
            }
            return result;
        }

        public ObservableCollection<ProblemItem> LoadAllProblems()
        {
            Problems = new ObservableCollection<ProblemItem>();
            foreach (var problemFile in ProblemFiles)
            {
                foreach (var problem in LoadProblemFromFile(problemFile))
                {
                    Problems.Add(problem);
                }
            }
            return Problems;
        }

        public List<LibraryItem> LoadLibraryFromFile(string libraryFilename)
        {
            string libraryPath = Path.Combine(Path.GetDirectoryName(WorkspacePath), LIBRARIES_PATH, libraryFilename);
            string workingDir = Directory.GetCurrentDirectory();
            string newFile = Path.Combine(workingDir, libraryFilename);
            var result = new List<LibraryItem>();
            List<Exception> exceptions = new List<Exception>();
            try
            {
                if (!File.Exists(newFile))
                {
                    File.Copy(libraryPath, newFile, false);
                }
                var assembly = Assembly.LoadFrom(newFile);
                var library = new LibraryItem(assembly.GetName(), libraryFilename);
                if (!result.Contains(library))
                {
                    result.Add(library);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Libraries = new ObservableCollection<LibraryItem>(result);
            return result;
        }

        public ObservableCollection<LibraryItem> LoadAllLibraries()
        {
            Libraries = new ObservableCollection<LibraryItem>();
            foreach (var libraryFile in LibraryFiles)
            {
                foreach (var library in LoadLibraryFromFile(libraryFile))
                {
                    Libraries.Add(library);
                }
            }
            return Libraries;
        }

        public void SolveProblem(int index)
        {
            var problem = Problems[index].Problem;
            if (problem.IsInputDataSet)
            {
                problem.Solve();
            }
        }

        public void SolveProblem(IProblem problem)
        {
            int index = Problems.IndexOf(Problems.First(x => x.Problem == problem));
            SolveProblem(index);
        }

        public IProblem GetProblem(IProblem problem)
        {
            int index = Problems.IndexOf(Problems.First(x => x.Problem == problem));
            return GetProblem(index);
        }

        public IProblem GetProblem(ProblemItem problemItem)
        {
            return GetProblem(Problems.IndexOf(problemItem));
        }

        public IProblem GetProblem(int index)
        {
            try
            {
                return Problems[index].Problem;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Can't find problem. Details:\n" + ex.Message);
            }
        }

        public void AddProblems(string[] fileNames)
        {
            foreach (var f in fileNames)
            {
                if (!ProblemFiles.Contains(f))
                {
                    ProblemFiles.Add(f);
                }
            }
        }

        public void AddLibraries(string[] fileNames)
        {
            foreach (var f in fileNames)
            {
                if (!LibraryFiles.Contains(f))
                {
                    LibraryFiles.Add(f);
                }
            }
            Workspace.Save(WorkspacePath, this);
        }

        public void RemoveProblem(string filename)
        {
            if (ProblemFiles.Contains(filename))
            {
                int index = ProblemFiles.IndexOf(filename);
                ProblemFiles.RemoveAt(index);
                if (Problems != null && Problems.Count >= index - 1)
                {
                    Problems.Remove(Problems[index]);
                }
            }
        }

        public void RemoveProblems(string[] problems)
        {
            foreach (var library in problems)
            {
                var libraryFile = LibraryFiles.First(x => System.IO.Path.GetFileName(x) == library);
                RemoveLibrary(libraryFile);
                LibraryFiles.Remove(libraryFile);
            }
            Workspace.Save(WorkspacePath, this);
        }

        public void RemoveLibrary(string filename)
        {
            string directoryPath = Path.Combine(Path.GetDirectoryName(WorkspacePath), LIBRARIES_PATH);
            if (!Directory.Exists(directoryPath))
            {
                return;
            }
            string name = Path.GetFileName(filename);
            File.Delete(Path.Combine(directoryPath, filename));
            LibraryFiles.Remove(name);
        }

        public void RemoveLibraries(string[] libraries)
        {
            foreach (var library in libraries)
            {
                var libraryFile = LibraryFiles.First(x => System.IO.Path.GetFileName(x) == library);
                RemoveLibrary(libraryFile);
                LibraryFiles.Remove(libraryFile);
            }
            Workspace.Save(WorkspacePath, this);
        }

        public void CopyLibraries(string[] files)
        {
            List<Exception> exceptions = new List<Exception>();
            foreach (var file in files)
            {
                try
                {
                    CopyLibrary(file);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                string message = "";
                foreach (var exception in exceptions)
                {
                    message += exception.Message + "\n";
                }
                throw new Exception(message);
            }
        }

        public void CopyLibrary(string file)
        {
            string filename = Path.GetFileName(file);
            string newFile = Path.Combine(WorkspacePath, PROBLEMS_PATH, filename);
            if (!File.Exists(newFile))
            {
                try
                {
                    File.Copy(file, newFile);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentException("File " + filename + " already exists.");
            }
        }

        public void CopyToDirectory(string directory, params string[] files)
        {
            string directoryPath = Path.Combine(Path.GetDirectoryName(WorkspacePath), directory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            foreach (var file in files)
            {
                var newFile = Path.Combine(directoryPath, Path.GetFileName(file));
                if (!File.Exists(newFile))
                {
                    File.Copy(file, newFile);
                }
            }
        }
    }
}
