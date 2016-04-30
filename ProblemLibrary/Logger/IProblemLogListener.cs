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
    public interface IProblemLogListener
    {
        /// <summary>
        /// Handle info message here
        /// </summary>
        /// <param name="message">message</param>
        void HandleInfoMessage(string message);

        /// <summary>
        /// Handle error message here
        /// </summary>
        /// <param name="message">error message</param>
        void HandleErrorMessage(string message);
    }
}