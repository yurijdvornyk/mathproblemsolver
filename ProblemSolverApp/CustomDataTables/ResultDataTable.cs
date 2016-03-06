using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemLibrary;
using System.Windows;

namespace ProblemSolverApp
{
    public class ResultDataTable : DataTable
    {
        public ProblemResult Result { get; set; }
        public const string DATA_TABLE_DEFAULT_NAME = "Results";
        public DataTable table = new DataTable(DATA_TABLE_DEFAULT_NAME);
        private DataColumn tableColumn = new DataColumn();
        private DataRow tableRow;

        public ResultDataTable() { }

        public ResultDataTable(ProblemResult result)
        {
            table = new DataTable(DATA_TABLE_DEFAULT_NAME);
            Result = result;
            FillDataTable();
        }

        public void FillDataTable()
        {
            table.Clear();
            table.Rows.Clear();
            table.Columns.Clear();

            switch (Result.Type)
            {
                case ProblemResultType.NumberOrArray:
                    handleD1();
                    break;
                case ProblemResultType.FunctionOfOneArgument:
                    handleD2();
                    break;
                case ProblemResultType.NumericMatrix:
                    handleMatrix();
                    break;
                case ProblemResultType.MatrixCollection:
                    handleMatrixCollection();
                    break;
                default:
                    break;
            }
        }

        private void handleD1()
        {
            string title = string.Empty;

            var title0 = Result.Title as string[];
            if (title0 != null)
            {
                if (!table.Columns.Contains(title0[0]))
                {
                    title = title0[0];
                }
            }
            else
            {
                var title1 = Result.Title as string;
                if (title1 != null)
                {
                    if (!table.Columns.Contains(title1))
                    {
                        title = title1;
                    }
                }
                else
                {
                    throw new ArgumentException("Unable to parse title.");
                }
            }

            if (title.Replace(" ", "") == string.Empty)
            {
                title = "Result";
            }

            tableColumn = new DataColumn();
            tableColumn.ColumnName = title;
            table.Columns.Add(tableColumn);

            var values = (object[])Result.Value;
            for (int row = 0; row < values.GetLength(0); ++row)
            {
                tableRow = table.NewRow();
                for (int col = 0; col < 1; ++col)
                {
                    var index = title;
                    tableRow[index] = values[row];
                }
                table.Rows.Add(tableRow);
            }
        }

        private void handleD2()
        {
            var title = (string[])Result.Title;
            foreach (var name in title)
            {
                if (!table.Columns.Contains(name))
                {
                    tableColumn = new DataColumn();
                    tableColumn.ColumnName = name;
                    table.Columns.Add(tableColumn);
                }
            }

            var values = (object[,])Result.Value;
            for (int row = 0; row < values.GetLength(0); ++row)
            {
                tableRow = table.NewRow();
                for (int col = 0; col < values.GetLength(1); ++col)
                {
                    var index = title[col].ToString();
                    tableRow[index] = values[row, col];
                }
                table.Rows.Add(tableRow);
            }
        }

        private void handleMatrix()
        {
            var values = (object[,])Result.Value;

            for (int j = 0; j < values.GetLength(1); ++j)
            {
                tableColumn = new DataColumn();
                tableColumn.ColumnName = (j + 1).ToString();
                table.Columns.Add(tableColumn);
            }

            for (int row = 0; row < values.GetLength(0); ++row)
            {
                tableRow = table.NewRow();
                for (int col = 0; col < values.GetLength(1); ++col)
                {
                    var index = (col + 1).ToString();
                    tableRow[index] = values[row, col];
                }
                table.Rows.Add(tableRow);
            }
        }

        private void handleMatrixCollection() { }

        public void ResetTable()
        {
            table = new DataTable(DATA_TABLE_DEFAULT_NAME);
            FillDataTable();
        }

        public DataView AsDataView { get { return table.AsDataView(); } }
    }
}