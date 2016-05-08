using ProblemDevelopmentKit.Listener;

namespace ProblemDevelopmentKit.Logger
{
    /// <summary>
    /// Use this class to log information while executing problem.
    /// </summary>
    public class ProblemLogger: Notifier<IProblemLogListener>
    {
        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="type">Message type.</param>
        /// <param name="message">Message content.</param>
        public static void Log(MessageType type, string message)
        {
            foreach (var listener in GetListeners())
            {
                listener.HandleMessage(type, message);
            }
        }
    }
}