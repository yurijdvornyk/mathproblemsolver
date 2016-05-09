using ProblemDevelopmentKit.Logger;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProblemSolverApp.Controls
{
    /// <summary>
    /// Interaction logic for TerminalControl.xaml
    /// </summary>
    public partial class TerminalControl : UserControl, IProblemLogListener
    {
        public TerminalControl()
        {
            InitializeComponent();
            messages = new List<string>();
            ProblemLogger.RegisterListener(this);
        }

        private List<string> messages;

        public string LoggerContent
        {
            get { return (string)GetValue(LoggerContentProperty); }
            set { SetValue(LoggerContentProperty, value); }
        }

        // Must be overridden from IProblemLogListener
        public int Filter { get; set; }

        // Using a DependencyProperty as the backing store for LoggerContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoggerContentProperty =
            DependencyProperty.Register("LoggerContent", typeof(string), typeof(TerminalControl), new PropertyMetadata(""));

        public void AddMessage(string content, bool inNewLine = true)
        {
            if (!inNewLine && messages != null && messages.Count > 0)
            {
                messages.RemoveAt(messages.Count - 1);
            }
            messages.Add(content);
            Dispatcher.Invoke(() => updateLoggerContent());
        }

        private void updateLoggerContent()
        {
            StringBuilder newLoggerContent = new StringBuilder();
            foreach (var message in messages)
            {
                newLoggerContent.AppendLine(message);
            }
            LoggerContent = newLoggerContent.ToString();
        }

        public void HandleMessage(MessageType type, string message)
        {
            bool inNewLine = true;
            if (type == MessageType.Status)
            {
                inNewLine = false;
            }
            AddMessage(string.Format("{0}: {1}", type, message), inNewLine);
        }
    }
}
