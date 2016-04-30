using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemLibrary.Logger
{
    public class ProblemLogger
    {
        private static List<IProblemLogListener> listeners;

        public static void RegisterListener(IProblemLogListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public static void UnregisterListener(IProblemLogListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        public static void LogInfo(string message)
        {
            foreach (var listener in listeners)
            {
                listener.HandleInfoMessage(message);
            }
        }

        public static void LogError(string message)
        {
            foreach (var listener in listeners)
            {
                listener.HandleErrorMessage(message);
            }
        }
    }
}