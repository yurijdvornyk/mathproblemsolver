using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.Manager.EventManager
{
    public class AppEventManager
    {
        private static List<IEventListener> listeners;

        public static void AddListener(IEventListener listener)
        {
            if (listeners == null)
            {
                listeners = new List<IEventListener>();
            }

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public static void RemoveListener(IEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public static void NotifyListeners(EventType eventType, params object[] args)
        {
            foreach (var listener in listeners)
            {
                listener.HandleEvent(eventType, args);
            }
        }
    }
}
