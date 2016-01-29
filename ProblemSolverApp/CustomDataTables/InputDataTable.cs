using ProblemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp
{
    public class InputDataTable: DataTable
    {
        public IProblem CurrentProblem { get; set; }
        public const string DATA_TABLE_DEFAULT_NAME = "InputData";
        public DataTable table = new DataTable(DATA_TABLE_DEFAULT_NAME);
        private DataColumn tableColumn = new DataColumn();
        private DataRow tableRow;

        public InputDataTable() { }

        public InputDataTable(IProblem problem)
        {
            CurrentProblem = problem;
            FillDataTable();
        }

        public void FillDataTable()
        {
            table.Clear();

            if (!table.Columns.Contains("Parameter"))
            {
                tableColumn = new DataColumn();
                tableColumn.ColumnName = "Parameter";
                table.Columns.Add(tableColumn);
            }

            if (!table.Columns.Contains("Type"))
            {
                tableColumn = new DataColumn();
                tableColumn.ColumnName = "Type";
                table.Columns.Add(tableColumn);
            }

            if (!table.Columns.Contains("Value"))
            {
                tableColumn = new DataColumn();
                tableColumn.ColumnName = "Value";
                table.Columns.Add(tableColumn);
            }

            for (int row = 0; row < CurrentProblem.InputData.Count; ++row)
            {
                tableRow = table.NewRow();
                tableRow["Parameter"] = CurrentProblem.InputData[row].Name;
                tableRow["Type"] = CurrentProblem.InputData[row].Type;
                tableRow["Value"] = CurrentProblem.InputData[row].Value;
                table.Rows.Add(tableRow);
            }
        }

        public void ResetTable()
        {
            table = new DataTable(DATA_TABLE_DEFAULT_NAME);
            FillDataTable();
        }

        public DataView AsDataView { get { return table.AsDataView(); } }
    }
}
