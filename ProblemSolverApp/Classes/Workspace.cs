using ProblemLibrary;
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

        public List<ProblemItem> LoadProblemsFromFile(string problemFileName)
        {
            string problemsPath = Path.GetDirectoryName(WorkspacePath) + Constants.PATH_SEPARATOR + "problems" + Constants.PATH_SEPARATOR + problemFileName;
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
                foreach (var problem in LoadProblemsFromFile(problemFile))
                {
                    Problems.Add(problem);
                }
            }
            return Problems;
        }

        internal void AddLibraries(string[] fileNames)
        {
            throw new NotImplementedException();
        }

        public List<LibraryItem> LoadLibrariesFromFile(string libraryFilename)
        {
            string libraryPath = Path.GetDirectoryName(WorkspacePath) + Constants.PATH_SEPARATOR + "libraries" + Constants.PATH_SEPARATOR + libraryFilename;
            string workingDir = Directory.GetCurrentDirectory();
            string newFile = workingDir + Constants.PATH_SEPARATOR + libraryFilename;
            var result = new List<LibraryItem>();
            List<Exception> exceptions = new List<Exception>();
            try
            {
                File.Copy(libraryPath, newFile);
                var assembly = Assembly.LoadFrom(newFile);
                var library = new LibraryItem(assembly.GetName());
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
                foreach (var library in LoadLibrariesFromFile(libraryFile))
                {
                    Libraries.Add(library);
                }
            }
            return Libraries;
        }

        public void Calculate(int index)
        {
            var problem = Problems[index].Problem;
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
            int index = Problems.IndexOf(Problems.First(x => x.Problem == problem));
            Calculate(index);
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

        // TODO: Improve
        public void AddProblems(params string[] fileFullNames)
        {
            foreach (var f in fileFullNames)
            {
                ProblemFiles.Add(f);
            }
            //List<Exception> exceptions = new List<Exception>();
            //foreach (var fileFullName in fileFullNames)
            //{
            //    try
            //    {
            //        var asm = Assembly.LoadFrom(fileFullName);
            //        var x = from type in asm.GetTypes()
            //                where typeof(IProblem).IsAssignableFrom(type)
            //                select type;
            //        if (x.Count() == 0)
            //        {
            //            exceptions.Add(new ArgumentException("Problem you try to load is not valid. It should implement IProblem interface."));
            //            continue;
            //        }

            //        string filename = Path.GetFileNameWithoutExtension(fileFullName);
            //        string newFileFullName = WorkspacePath + Constants.PATH_SEPARATOR + "problems" + Constants.PATH_SEPARATOR + Path.GetFileName(fileFullName);


            //        // We should update assembly file
            //        asm = Assembly.LoadFrom(newFileFullName);
            //        var problemInfoItem = new ProblemItem(
            //            (IProblem)Activator.CreateInstance(x.First(y => typeof(IProblem).IsAssignableFrom(y))),
            //            asm);
            //        Problems.Add(problemInfoItem);
            //        //ProblemList.Add(problemInfoItem.Problem);
            //    }
            //    catch (Exception ex)
            //    {
            //        exceptions.Add(ex);
            //        continue;
            //    }
            //}

            //string message = string.Empty;
            //if (exceptions.Count > 0)
            //{
            //    foreach (var ex in exceptions)
            //    {
            //        message += ex.Message + "\n";
            //    }
            //    throw new Exception(message);
            //}
        }

        public void RemoveProblem(string filename)
        {
            int index = ProblemFiles.IndexOf(filename);
            Problems.Remove(Problems[index]);
        }

        // TODO: Improve
        public void RemoveLibrary(string filename)
        {
            int index = LibraryFiles.IndexOf(filename);
            Libraries.Remove(Libraries[index]);
        }
    }
}
