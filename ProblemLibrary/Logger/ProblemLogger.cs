﻿using ProblemLibrary.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemLibrary.Logger
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