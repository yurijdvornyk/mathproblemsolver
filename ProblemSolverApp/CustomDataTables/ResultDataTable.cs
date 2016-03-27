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
        public ProblemResultTableValue ResultValue { get; set; }
        public const string DATA_TABLE_DEFAULT_NAME = "Results";
        public DataTable Table = new DataTable(DATA_TABLE_DEFAULT_NAME);
        private DataColumn tableColumn = new DataColumn();
        private DataRow tableRow;

        public ResultDataTable() { }

        public ResultDataTable(ProblemResultTableValue result)
        {
            Table = new DataTable(DATA_TABLE_DEFAULT_NAME);
            ResultValue = result;
            FillDataTable();
        }

        private void FillDataTable()
        {
            if (ResultValue == null)
            {
                throw new ArgumentNullException("Result is null now");
            }
            for (int i = 0; i < ResultValue.Titles.Length; ++i)
            {
                tableColumn = new DataColumn();
                tableColumn.ColumnName = String.IsNullOrEmpty(ResultValue.Titles[i]) ? (i + 1).ToString() : ResultValue.Titles[i];
                Table.Columns.Add(tableColumn);
            }

            for (int i = 0; i < ResultValue.Values.GetLength(0); ++i)
            {
                tableRow = Table.NewRow();
                for (int j = 0; j < ResultValue.Values.GetLength(1); ++j)
                {
                    tableRow[j] = ResultValue.Values[i, j];
                }
                Table.Rows.Add(tableRow);
            }
        }

        public void ResetTable()
        {
            Table = new DataTable(DATA_TABLE_DEFAULT_NAME);
            FillDataTable();
        }

        public DataView AsDataView { get { return Table.AsDataView(); } }
    }
}