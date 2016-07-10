using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemDevelopmentKit.Result
{
    public class TableResultItem : IResultItem
    {
        public string Title { get; set; }

        public List<string> ColumnTitles { get; private set; }

        public List<List<object>> Value { get; set; }

        public TableResultItem() : this(string.Empty) { }

        public TableResultItem(string title)
        {
            Title = title;
            ColumnTitles = new List<string>();
            Value = new List<List<object>>();
        }

        public void SetResult(object[,] result)
        {
            Value = convertMatrixToLists(result);
        }

        public object[,] GetValueAsMatrix()
        {
            return convertListsToMatrix(Value);
        }

        #region Convertors

        private static object[,] convertListsToMatrix(List<List<object>> value)
        {
            int rows = value.Count;
            int columns = 0;
            if (rows > 0)
            {
                columns = value[0].Count;
            }

            object[,] result = new object[rows, columns];

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; i < columns; ++j)
                {
                    result[i, j] = value[i][j];
                }
            }
            return result;
        }

        private static List<List<object>> convertMatrixToLists(object[,] objectMatrix)
        {
            List<List<object>> result = new List<List<object>>();
            for (int i = 0; i < objectMatrix.GetLength(0); ++i)
            {
                List<object> row = new List<object>();
                for (int j = 0; j < objectMatrix.GetLength(1); ++j)
                {
                    row.Add(objectMatrix[i, j]);
                }
                result.Add(row);
            }
            return result;
        }

        #endregion
    }
}