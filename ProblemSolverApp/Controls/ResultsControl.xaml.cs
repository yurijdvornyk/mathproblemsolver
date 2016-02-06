using ProblemLibrary;
using System;
using System.Collections.Generic;
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
using SpotLibrary;
using ProblemSolverApp.Classes.CustomLogger;

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

            Logger = CustomLogger.GetInstance();

            _ResultDataTable = new ResultDataTable();
            dataGrid.ItemsSource = _ResultDataTable.AsDataView;

            _InputDataTable = new InputDataTable();
            inputDataGrid.ItemsSource = _InputDataTable.AsDataView;
        }

        public IProblem CurrentProblem { get; set; }

        public ResultDataTable _ResultDataTable { get; set; }
        public InputDataTable _InputDataTable { get; set; }

        public CustomLogger Logger { get; set; }

        public void UpdateResults()
        {
            var result = CurrentProblem.Result;

            // UPDATE TABLE
            _ResultDataTable.Result = result;
            _ResultDataTable.FillDataTable();
            dataGrid.ItemsSource = _ResultDataTable.AsDataView;
            // LOG Table updated

            // UPDATE PLOT
            spotControl.ClearGraphs();

            try {
                switch (result.Type)
                {
                    case ProblemResultType.D1:
                        handleD1(result);
                        break;
                    case ProblemResultType.D2:
                        handleD2(result);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                MessageBox.Show("There was an error while handling problem results.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // LOG Plot updated

            // UPDATE INPUT DATA
            _InputDataTable.CurrentProblem = CurrentProblem;
            _InputDataTable.FillDataTable();
            inputDataGrid.ItemsSource = _InputDataTable.AsDataView;
            spotControl.SpotName = CurrentProblem.Name;
        }

        private void handleD1(ProblemResult result)
        {
            var values = result.Value as object[];
            if (values != null)
            {
                foreach (var value in values)
                {
                    spotControl.AddGraph(
                        new List<Point>() { new Point(double.Parse(value.ToString()), 0) },
                        Brushes.DarkBlue, 2);
                }
            }
        }

        private void handleD2(ProblemResult result)
        {
            var values = (object[,])result.Value;
            if (values.GetLength(1) == 2)
            {
                List<Point> points = new List<Point>();
                try {
                    for (int i = 0; i < values.GetLength(0); ++i)
                    {
                        try {
                            points.Add(new Point(
                                double.Parse(values[i, 0].ToString()),
                                double.Parse(values[i, 1].ToString())));
                        } catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                var titles = (string[])result.Title;
                spotControl.HorizontalAxisName = titles[0];
                spotControl.VerticalAxisName = titles[1];
                spotControl.AddGraph(points, Brushes.DarkRed, 2);
            }
        }
    }
}