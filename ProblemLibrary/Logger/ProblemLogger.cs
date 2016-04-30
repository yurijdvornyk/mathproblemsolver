using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemLibrary.Logger
{
    /// <summary>
    /// Use this class to log information while executing problem
    /// </summary>
    public class ProblemLogger
    {
        private static List<IProblemLogListener> listeners;

        /// <summary>
        /// Make you listener listen new logs appears
        /// </summary>
        /// <param name="listener">listener</param>
        public static void RegisterListener(IProblemLogListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        /// <summary>
        /// Make you listener not to listen new logs appears
        /// </summary>
        /// <param name="listener">listener</param>
        public static void UnregisterListener(IProblemLogListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }

        /// <summary>
        /// Log info message
        /// </summary>
        /// <param name="message">message</param>
        public static void LogInfo(string message)
        {
            foreach (var listener in listeners)
            {
                listener.HandleInfoMessage(message);
            }
        }

        /// <summary>
        /// Log error message. Use this for handling exceptions, writing details for some errors and so on.
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            foreach (var listener in listeners)
            {
                listener.HandleErrorMessage(message);
            }
        }
    }
}