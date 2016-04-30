using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.Manager.EventManager
{
    public interface IEventListener
    {
        void HandleEvent(EventType eventType, params object[] args);
    }
}
