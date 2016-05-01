using ProblemLibrary.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemLibrary.Logger
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