using ProblemSolverApp.Classes.CustomLogger;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProblemSolverApp.Controls
{
    /// <summary>
    /// Interaction logic for TerminalControl.xaml
    /// </summary>
    public partial class TerminalControl : UserControl
    {
        public TerminalControl()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)lbOutput.Items).CollectionChanged += lbOutput_CollectionChanged;
        }

        private void lbOutput_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // scroll the new item into view   
                lbOutput.ScrollIntoView(e.NewItems[0]);
            }
        }

        public CustomLogger Logger
        {
            get { return (CustomLogger)GetValue(LoggerProperty); }
            set { SetValue(LoggerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Logger.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoggerProperty =
            DependencyProperty.Register("Logger", typeof(CustomLogger), typeof(TerminalControl), new PropertyMetadata(CustomLogger.GetInstance()));

        private void btnCopyLoggerMessageItem_Click(object sender, RoutedEventArgs e)
        {
            var message = lbOutput.SelectedItem as CustomLoggerMessage;
            if (message != null)
            {
                Clipboard.SetText(message.ToString());
            }
        }

        private void btnCopyLoggerMessage_Click(object sender, RoutedEventArgs e)
        {
            var message = lbOutput.SelectedItem as CustomLoggerMessage;
            if (message != null)
            {
                Clipboard.SetText(message.Message);
            }
        }
    }
}
