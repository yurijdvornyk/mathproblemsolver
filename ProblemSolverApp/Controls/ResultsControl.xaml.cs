using ProblemDevelopmentKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using ProblemSolverApp.Classes.Utils;
using Microsoft.Win32;
using ProblemSolverApp.Classes.Manager;
using System.IO;

namespace ProblemSolverApp.Controls
{
    /// <summary>
    /// Interaction logic for ResultsControl.xaml
    /// </summary>
    public partial class ResultsControl : UserControl
    {
        public ResultsControl()
        {
            InitializeComponent();
            random = new Random();

            _InputDataTable = new InputDataTable();
            inputDataGrid.ItemsSource = _InputDataTable.AsDataView;
        }

        public ObservableCollection<TabItem> ResultTabs
        {
            get { return (ObservableCollection<TabItem>)GetValue(ResultTabsProperty); }
            set { SetValue(ResultTabsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResultTabs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultTabsProperty =
            DependencyProperty.Register("ResultTabs", typeof(ObservableCollection<TabItem>), 
                typeof(ResultsControl), new PropertyMetadata(new ObservableCollection<TabItem>()));

        public IProblem CurrentProblem { get; set; }
        public InputDataTable _InputDataTable { get; set; }
        private Random random;

        public void UpdateResults()
        {
            // TODO: try/ctach ???

            var result = CurrentProblem.Result;
            updateTable(result);
            updateInputData();
            // LOG Table updated

            // UPDATE PLOT
            updatePlot(result);
            // LOG Plot updated

            if (String.IsNullOrEmpty(result.Comments))
            {
                wbCommentsBrowser.NavigateToString("<p>No any comments here.");
            }
            else
            {
                wbCommentsBrowser.NavigateToString(result.Comments);
            }
        }

        private void updateTable(ProblemResult result)
        {
            ResultTabs.Clear();
            for (int i = 0; i < result.TableResult.Values.Count; ++i)
            {
                ResultDataTable resultDataTable = new ResultDataTable(result.TableResult.Values[i]);
                DataGrid dataGrid = new DataGrid();
                dataGrid.IsReadOnly = true;
                dataGrid.CanUserSortColumns = false;
                dataGrid.CanUserAddRows = false;
                dataGrid.CanUserDeleteRows = false;
                dataGrid.CanUserResizeRows = false;
                dataGrid.CanUserResizeColumns = false;
                dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                dataGrid.ItemsSource = resultDataTable.AsDataView;

                TabItem tabItem = new TabItem();
                tabItem.Header = 
                    string.IsNullOrEmpty(result.TableResult.Values[i].Title) 
                    ? (i + 1).ToString() : result.TableResult.Values[i].Title;
                tabItem.Content = dataGrid;
                ResultTabs.Add(tabItem);
            }

            if (ResultTabs.Count > 0)
            {
                tcResultsTabs.SelectedIndex = 0;
                tcResultsTabs.SelectedItem = ResultTabs.First();
                ResultTabs.First().IsSelected = true;
            }
        }

        private void updatePlot(ProblemResult result)
        {
            spotControl.ClearGraphs();
            foreach (var value in result.VisualResult.Graphs)
            {
                List<Point> points = new List<Point>();
                for (int i = 0; i < value.Keys.Count; ++i)
                {
                    points.Add(new Point(value.Keys[i], value.Values[i]));
                }
                spotControl.AddGraph(points, getRandomColor(Colors.LightGray), 2, value.Title);
            }
            if (result.VisualResult.Graphs.Count > 1)
            {
                spotControl.ShowGraphInfo = true;
            }
            else
            {
                spotControl.ShowGraphInfo = false;
            }
        }

        private void updateInputData()
        {
            _InputDataTable.CurrentProblem = CurrentProblem;
            _InputDataTable.FillDataTable();
            inputDataGrid.ItemsSource = _InputDataTable.AsDataView;
            spotControl.SpotName = CurrentProblem.Name;
        }

        private SolidColorBrush getRandomColor(Color mix)
        {
            int red = random.Next(256);
            int green = random.Next(256);
            int blue = random.Next(256);
            Color color = new Color();
            color.R = (byte) red;
            color.G = (byte) green;
            color.B = (byte) blue;
            color.A = 255;
            return new SolidColorBrush(color);
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = "" + DateTime.Now.ToString("yyyy-MM-d_hh-mm-ss") + " " + CurrentProblem.Name + ".xlsx";
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Microsoft Excel files (.xlsx)|*.xlsx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    MessageBox.Show("This feature will be added later.");
                    // TODO: Add
                    //FileUtils.SaveProblemToXls(CurrentProblem, dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnExportResult_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = "" + DateTime.Now.ToString("yyyy-MM-d_hh-mm-ss") + " " + CurrentProblem.Name + ".tex";
            dlg.DefaultExt = ".tex";
            dlg.Filter = "LaTeX files (.tex)|*.tex";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    string texText = ExportUtils.getTexMarkupForProblem(CurrentProblem, spotControl.ShowGraphInfo);
                    using (StreamWriter outfile = new StreamWriter(dlg.FileName))
                    {
                        outfile.Write(texText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}