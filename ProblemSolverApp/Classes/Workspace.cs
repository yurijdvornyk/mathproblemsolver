using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        [XmlArray(ElementName = "libraries")]
        [XmlArrayItem(ElementName = "library")]
        public ObservableCollection<string> LibraryFiles { get; }

        [XmlArray(ElementName = "problems")]
        [XmlArrayItem(ElementName = "problem")]
        public ObservableCollection<string> ProblemFiles { get; }

        public Workspace(): this("", "", null, null) { }

        public Workspace(string name, string description) : this(name, description, null, null) { }

        public Workspace(string name, string description, List<string> problemFiles, List<string> libraryFiles)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;

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
                Workspace workspace = (Workspace) xmlSerializer.Deserialize(reader);
                reader.Close();
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
    }
}
