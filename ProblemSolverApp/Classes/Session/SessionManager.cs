using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.Session
{
    public class SessionManager
    {
        private static SessionManager Session;

        public Workspace CurrentWorkspace { get; private set; }

        public AppDomain CurrentProblemDomain { get; private set; }

        private SessionManager()
        {
            CurrentWorkspace = null;
            CurrentProblemDomain = null;
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
            if (CurrentWorkspace != null && !string.IsNullOrEmpty(CurrentWorkspace.Name))
            {
                CurrentProblemDomain = AppDomain.CreateDomain(CurrentWorkspace.Name);
            }
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
            Workspace.Close(CurrentWorkspace, CurrentProblemDomain);
            CurrentWorkspace = null;
            CurrentProblemDomain = null;
        }

        #endregion
    }
}