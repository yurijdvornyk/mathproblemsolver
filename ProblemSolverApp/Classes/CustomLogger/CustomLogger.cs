using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.CustomLogger
{
    public class CustomLogger: INotifyPropertyChanged
    {
        private static CustomLogger customLogger;

        public ObservableCollection<CustomLoggerMessage> Messages { get; set; }

        public string CurrentStatus
        {
            get
            {
                if (Messages != null)
                {
                    if (Messages.Count > 0)
                    {
                        return Messages.Last().Message.Replace("\n", " ");
                    }
                }
                return "";
            }
        }

        #region Constructor

        private CustomLogger()
        {
            Messages = new ObservableCollection<CustomLoggerMessage>();
        }

        public static CustomLogger GetInstance()
        {
            if (customLogger == null)
            {
                customLogger = new CustomLogger();
            }
            return customLogger;
        }

        #endregion

        public void LogMessage(CustomLoggerMessageType type, DateTime time, string message)
        {
            Messages.Add(new CustomLoggerMessage(type, time, message));
            OnPropertyChanged("CurrentStatus");
        }

        public void LogSuccess(string message)
        {
            LogMessage(CustomLoggerMessageType.Success, DateTime.Now, message);
        }

        public void LogInfo(string message)
        {
            LogMessage(CustomLoggerMessageType.Info, DateTime.Now, message);
        }

        public void LogError(string message)
        {
            LogMessage(CustomLoggerMessageType.Error, DateTime.Now, message);
        }

        public void LogWarning(string message)
        {
            LogMessage(CustomLoggerMessageType.Warning, DateTime.Now, message);
        }

        #region Make properties change dynamically

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
