using Microsoft.Win32;
using ProblemLibrary;
using ProblemSolverApp.Classes;
using ProblemSolverApp.Classes.CustomLogger;
using ProblemSolverApp.Classes.Manager;
using ProblemSolverApp.Classes.Manager.EventManager;
using ProblemSolverApp.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for WorkspaceControl.xaml
    /// </summary>
    public partial class WorkspaceControl : UserControl, IEventListener
    {
        public WorkspaceControl()
        {
            InitializeComponent();
        }

        public Workspace CurrentWorkspace
        {
            get { return (Workspace)GetValue(CurrentWorkspaceProperty); }
            set { SetValue(CurrentWorkspaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentWorkspace.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentWorkspaceProperty =
            DependencyProperty.Register("CurrentWorkspace", typeof(Workspace), typeof(WorkspaceControl), new PropertyMetadata(null));

        public IProblem CurrentProblem
        {
            get
            {
                try
                {
                    return CurrentWorkspace.GetProblem(cbProblemSelector.SelectedItem as ProblemItem);
                }
                catch
                {
                    return null;
                }
            }
        }

        private const string UI_ELEMENT_NAME_PREFIX = "InputDataElement";

        public void UpdateControlLayout()
        {
            var currentProblem = CurrentWorkspace.Problems[cbProblemSelector.SelectedIndex].Problem;
            if (currentProblem == null)
            {
                return;
            }
            var data = currentProblem.InputData;
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            for (int i = 0; i < data.Count; ++i)
            {
                grid.RowDefinitions.Add(new RowDefinition());

                TextBlock key = new TextBlock();
                key.Text = data[i].Name;

                key.Margin = new Thickness(5, 1, 5, 1);
                Grid.SetColumn(key, 0);
                Grid.SetRow(key, grid.RowDefinitions.Count - 1);
                grid.Children.Add(key);

                FrameworkElement value = fillUILayoutEmptyValue(data[i]);
                value.Name = UI_ELEMENT_NAME_PREFIX + i.ToString();
                value.HorizontalAlignment = HorizontalAlignment.Stretch;
                string toolTip = "[" + data[i].Type + "]";
                if (data[i].IsRequired)
                {
                    toolTip += "*";
                }
                value.ToolTip = toolTip;
                value.Margin = new Thickness(5, 1, 5, 1);

                Grid.SetColumn(value, 1);
                Grid.SetRow(value, grid.RowDefinitions.Count - 1);
                grid.Children.Add(value);
            }
        }

        private FrameworkElement fillUILayoutEmptyValue(ProblemDataItem problemDataItem)
        {
            if (problemDataItem == null)
            {
                return new FrameworkElement();
            }

            var defaultValue = problemDataItem.DefaultValue;
            var value = problemDataItem.Value;

            switch (problemDataItem.Type)
            {
                case ProblemDataItemType.Boolean:
                    CheckBox checkBox = new CheckBox();
                    return checkBox;

                case ProblemDataItemType.String:
                    TextBox textBoxString = new TextBox();
                    return textBoxString;

                case ProblemDataItemType.Function:
                    TextBox textBoxFunction = new TextBox();
                    return textBoxFunction;

                case ProblemDataItemType.Character:
                    TextBox textBoxChar = new TextBox();
                    return textBoxChar;

                case ProblemDataItemType.Double:
                    TextBox textBoxDouble = new TextBox();
                    return textBoxDouble;

                case ProblemDataItemType.Int:
                    TextBox textBoxInt = new TextBox();
                    return textBoxInt;

                case ProblemDataItemType.UnsignedInt:
                    TextBox textBoxUint = new TextBox();
                    return textBoxUint;

                case ProblemDataItemType.OneOfMany:
                    ComboBox comboBox = new ComboBox();
                    try
                    {
                        var items = problemDataItem.ExtraData as List<object>;
                        foreach (var i in items)
                        {
                            comboBox.Items.Add(i.ToString());
                        }
                        return comboBox;
                    }
                    catch
                    {
                        return new FrameworkElement();
                    }

                case ProblemDataItemType.SomeOfMany:
                    ListBox listBox = new ListBox();
                    listBox.MaxHeight = 80;
                    try
                    {
                        var items = problemDataItem.ExtraData as List<object>;
                        foreach (var i in items)
                        {
                            listBox.Items.Add(i.ToString());
                        }
                        return listBox;
                    }
                    catch
                    {
                        return new FrameworkElement();
                    }

                default:
                    return new FrameworkElement();
            }
        }

        private void fillLayoutWithValues(IProblem problem)
        {
            var data = problem.InputData;
            for (int i = 0; i < data.Count; ++i)
            {
                bool success = false;
                foreach (var j in grid.Children)
                {
                    var element = j as FrameworkElement;
                    if (element != null)
                    {
                        if (element.Name == UI_ELEMENT_NAME_PREFIX + i.ToString())
                        {
                            if (problem.IsInputDataSet)
                            {
                                if (data[i].Value != null)
                                {
                                    setElementValue(element, data[i].Type, data[i].Value);
                                }
                            }
                            else
                            {
                                if (data[i].DefaultValue != null)
                                {
                                    setElementValue(element, data[i].Type, data[i].DefaultValue);
                                }
                            }
                        }
                    }
                }
                if (!success)
                {
                    // LOG error: can't find...
                }
            }
        }

        private void setElementValue(FrameworkElement element, ProblemDataItemType type, object value)
        {
            switch (type)
            {
                case ProblemDataItemType.Function:
                    (element as TextBox).Text = value.ToString();
                    break;

                case ProblemDataItemType.Boolean:
                    (element as CheckBox).IsChecked = (bool)value;
                    break;

                case ProblemDataItemType.String:
                    (element as TextBox).Text = value.ToString();
                    break;

                case ProblemDataItemType.Character:
                    (element as TextBox).Text = value.ToString();
                    break;

                case ProblemDataItemType.Double:
                    (element as TextBox).Text = value.ToString();
                    break;

                case ProblemDataItemType.Int:
                    (element as TextBox).Text = value.ToString();
                    break;

                case ProblemDataItemType.UnsignedInt:
                    (element as TextBox).Text = value.ToString();
                    break;

                case ProblemDataItemType.OneOfMany:
                    try {
                        (element as ComboBox).SelectedItem = value;
                    }
                    catch
                    {
                        (element as ComboBox).SelectedIndex = -1;
                    }
                    break;

                default:
                    break;
            }
        }

        private object getElementValue(FrameworkElement element, ProblemDataItemType type)
        {
            switch (type)
            {
                case ProblemDataItemType.Boolean:
                    return (element as CheckBox).IsChecked.Value;

                case ProblemDataItemType.String:
                    return (element as TextBox).Text;

                case ProblemDataItemType.Function:
                    return (element as TextBox).Text;

                case ProblemDataItemType.Character:
                    return (element as TextBox).Text;

                case ProblemDataItemType.Double:
                    return (element as TextBox).Text;

                case ProblemDataItemType.Int:
                    return (element as TextBox).Text;

                case ProblemDataItemType.UnsignedInt:
                    return (element as TextBox).Text;

                case ProblemDataItemType.OneOfMany:
                    return (element as ComboBox).SelectedItem;

                default:
                    return null;
            }
        }

        public void ReloadWorkspace()
        {
            CurrentWorkspace = SessionManager.GetSession().CurrentWorkspace;
            updateWorkspaceDescription();
        }

        private void updateWorkspaceDescription()
        {
            if (CurrentWorkspace != null)
            {
                if (string.IsNullOrEmpty(CurrentWorkspace.Description))
                {
                    webBrowser.NavigateToString("No description for this workspace");
                }
                else
                {
                    webBrowser.NavigateToString(CurrentWorkspace.Description);
                }
            }
        }

        #region Events

        private void cbProblemSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProblemSelector.SelectedItem != null)
            {
                var problem = ((ProblemItem)cbProblemSelector.SelectedItem).Problem;

                if (!string.IsNullOrEmpty(problem.Equation)){
                    string url = @"http://chart.apis.google.com/chart?cht=tx&chf=bg,s,AAAAAA00&chs=" + 60 + "&chl=" + problem.Equation.Replace("+", "%2B");
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imgEquation.Source = bitmap;
                }

                UpdateControlLayout();
                fillLayoutWithValues(problem);
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProblem == null)
            {
                MessageBox.Show("Can't apply inpud data: problem is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var data = CurrentProblem.InputData;
            var values = new List<object>();

            for (int i = 0; i < data.Count; ++i)
            {
                foreach (var g in grid.Children)
                {
                    if (g is FrameworkElement)
                    {
                        var element = g as FrameworkElement;
                        if (element.Name == UI_ELEMENT_NAME_PREFIX + i.ToString())
                        {
                            values.Add(getElementValue(element, data[i].Type));
                        }
                    }
                }
            }

            try
            {
                CurrentProblem.SetInputData(values);
            }
            catch (Exception ex)
            {
                string message = "There was an error while setting input data. Details:\n" + ex.Message;
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentProblem.ResetInputData();
                UpdateControlLayout();
                fillLayoutWithValues(CurrentProblem);
                MessageBox.Show("Success");
                // LOG success
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // LOG error
            }
        }

        private void btnOpenWorkspaceEditor_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceEditorWindow window = new WorkspaceEditorWindow();
            window.Show();
        }

        void IEventListener.HandleEvent(EventType eventType, params object[] args)
        {
            switch (eventType)
            {
                case EventType.UpdateWorkspace:
                    ReloadWorkspace();
                    break;
                case EventType.OpenWorkspace:
                    if (args != null && args.Length > 0)
                    {
                        SessionManager.GetSession().OpenWorkspace(args[0].ToString());
                        ReloadWorkspace();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void btnAddProblemFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Dynamic-link library (.dll)|*.dll|All files (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CurrentWorkspace.CopyToDirectory(Workspace.PROBLEMS_PATH, dialog.FileNames);
                    List<string> filenames = new List<string>();
                    foreach (var file in dialog.FileNames)
                    {
                        filenames.Add(System.IO.Path.GetFileName(file));
                    }
                    CurrentWorkspace.AddProblems(filenames.ToArray());
                    string path = CurrentWorkspace.WorkspacePath;
                    SessionManager.GetSession().CloseWorkspace();
                    SessionManager.GetSession().OpenWorkspace(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemoveProblemFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> libraries = new List<string>();
                foreach (var library in lbProblems.SelectedItems)
                {
                    libraries.Add(library.ToString()); // TODO: Fix if we bind list element to ProblemItem object
                }
                CurrentWorkspace.RemoveProblems(libraries.ToArray());
                string path = CurrentWorkspace.WorkspacePath;
                SessionManager.GetSession().CloseWorkspace();
                SessionManager.GetSession().OpenWorkspace(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddLibraryFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dll";
            dialog.Filter = "Dynamic-link library (.dll)|*.dll|All files (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    CurrentWorkspace.CopyToDirectory(Workspace.LIBRARIES_PATH, dialog.FileNames);
                    List<string> filenames = new List<string>();
                    foreach (var file in dialog.FileNames)
                    {
                        filenames.Add(System.IO.Path.GetFileName(file));
                    }
                    CurrentWorkspace.AddLibraries(filenames.ToArray());
                    string path = CurrentWorkspace.WorkspacePath;
                    SessionManager.GetSession().CloseWorkspace();
                    SessionManager.GetSession().OpenWorkspace(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRemoveLibraryFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> libraries = new List<string>();
                foreach (var library in lbLibraries.SelectedItems)
                {
                    libraries.Add(library.ToString()); // TODO: Fix if we bind list element to LibraryItem object
                }
                CurrentWorkspace.RemoveLibraries(libraries.ToArray());
                string path = CurrentWorkspace.WorkspacePath;
                SessionManager.GetSession().CloseWorkspace();
                SessionManager.GetSession().OpenWorkspace(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}