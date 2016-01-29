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
            _ResultDataTable = new ResultDataTable();
            dataGrid.ItemsSource = _ResultDataTable.AsDataView;

            _InputDataTable = new InputDataTable();
            inputDataGrid.ItemsSource = _InputDataTable.AsDataView;
        }

        public IProblem CurrentProblem { get; set; }

        public ResultDataTable _ResultDataTable { get; set; }
        public InputDataTable _InputDataTable { get; set; }

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

            if (result.Type == ProblemResultType.Matrix)
            {
                var values = (object[,])result.Value;
                if (values.GetLength(1) == 2)
                {
                    List<Point> points = new List<Point>();
                    for (int i = 0; i < values.GetLength(0); ++i)
                    {
                        points.Add(new Point(
                            double.Parse(values[i, 0].ToString()),
                            double.Parse(values[i, 1].ToString())));
                    }

                    var titles = (string[])result.Title;
                    spotControl.HorizontalAxisName = titles[0];
                    spotControl.VerticalAxisName = titles[1];
                    spotControl.AddGraph(points, Brushes.DarkRed, 2);
                }
            }
            // LOG Plot updated

            // UPDATE INPUT DATA
            _InputDataTable.CurrentProblem = CurrentProblem;
            _InputDataTable.FillDataTable();
            inputDataGrid.ItemsSource = _InputDataTable.AsDataView;
        }
    }
}
