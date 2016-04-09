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
        public string WorkspacePath { get; private set; }

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

        public static void Close(Workspace workspace, AppDomain domain)
        {
            workspace = null;
            AppDomain.Unload(domain);
        }

        public List<ProblemItem> LoadProblemsFromFile(string problemPath)
        {
            var asm = Assembly.LoadFrom(problemPath);
            var result = new List<ProblemItem>();
            foreach (var type in asm.GetTypes())
            {
                if (typeof(IProblem).IsAssignableFrom(type))
                {
                    result.Add(new ProblemItem((IProblem)Activator.CreateInstance(type), asm));
                }
            }
            Problems = new ObservableCollection<ProblemItem>(result);
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

        public List<LibraryItem> LoadLibrariesFromFile(string libraryPath)
        {
            string filename = Path.GetFileName(libraryPath);
            string workingDir = Directory.GetCurrentDirectory();
            string newFile = workingDir + Constants.PATH_SEPARATOR + filename;
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
    }
}
