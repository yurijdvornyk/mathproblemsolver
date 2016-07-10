using System;
using System.Data;
using ProblemDevelopmentKit.Result;

namespace ProblemSolverApp
{
    public class ResultDataTable : DataTable
    {
        public TableResultItem ResultValue { get; set; }
        public const string DATA_TABLE_DEFAULT_NAME = "Results";
        public DataTable Table = new DataTable(DATA_TABLE_DEFAULT_NAME);
        private DataColumn tableColumn = new DataColumn();
        private DataRow tableRow;

        public ResultDataTable() { }

        public ResultDataTable(TableResultItem result)
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
            for (int i = 0; i < ResultValue.ColumnTitles.Count; ++i)
            {
                tableColumn = new DataColumn();
                tableColumn.ColumnName = 
                    string.IsNullOrEmpty(ResultValue.ColumnTitles[i]) ? 
                    (i + 1).ToString() : ResultValue.ColumnTitles[i];
                Table.Columns.Add(tableColumn);
            }

            object[,] value = ResultValue.GetValueAsMatrix();
            for (int i = 0; i < value.GetLength(0); ++i)
            {
                tableRow = Table.NewRow();
                for (int j = 0; j < ResultValue.GetValueAsMatrix().GetLength(1); ++j)
                {
                    tableRow[j] = value[i, j];
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