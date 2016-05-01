using ProblemDevelopmentKit.Listener;

namespace ProblemDevelopmentKit.Logger
{
    /// <summary>
    /// Implement this interface to listen to problem logs.
    /// </summary>
    public interface IProblemLogListener: IListener
    {
        /// <summary>
        /// Handle log messages
        /// </summary>
        /// <param name="type">Message type</param>
        /// <param name="message">Message content</param>
        void HandleMessage(MessageType type, string message);
    }
}