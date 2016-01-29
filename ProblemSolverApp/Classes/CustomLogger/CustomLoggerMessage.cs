using System;

namespace ProblemSolverApp.Classes.CustomLogger
{
    public class CustomLoggerMessage
    {
        public CustomLoggerMessageType Type { get; protected set; }
        public DateTime Time { get; protected set; }
        public string Message { get; protected set; }

        public CustomLoggerMessage(string message) : this(CustomLoggerMessageType.None, DateTime.Now, message) { }

        public CustomLoggerMessage(CustomLoggerMessageType type, string message) : this(type, DateTime.Now, message) { }

        public CustomLoggerMessage(CustomLoggerMessageType type, DateTime time, string message)
        {
            Type = type;
            Time = time;
            Message = message;
        }

        public override string ToString()
        {
            return "[" + Type + "] (" + DateTime.Now + ") " + Message;
        }
    }
}