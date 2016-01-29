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
            if (Result.Type == ProblemResultType.Matrix)
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
        }

        public void ResetTable()
        {
            table = new DataTable(DATA_TABLE_DEFAULT_NAME);
            FillDataTable();
        }

        public DataView AsDataView { get { return table.AsDataView(); } }
    }
}