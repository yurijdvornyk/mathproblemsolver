using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemDevelopmentKit.Result.Builder
{
    public class TableResultBuilder : IResultBuilder
    {
        private TableResult tableResult;
        private TableResultItem currentTable;

        private TableResultBuilder() : this(string.Empty) { }

        private TableResultBuilder(string title)
        {
            tableResult = new TableResult(title);
        }

        public static TableResultBuilder Create(string title = "")
        {
            return new TableResultBuilder(title);
        }

        public TableResultBuilder SetTableResult(TableResult tableResult)
        {
            this.tableResult = tableResult;
            return this;
        }

        public TableResult Build()
        {
            return tableResult;
        }

        public TableResultBuilder NewTable(string title = "")
        {
            currentTable = new TableResultItem(title);
            return this;
        }

        public TableResultBuilder NewRow()
        {
            currentTable.Value.Add(new List<object>());
            return this;
        }

        public TableResultBuilder AddValue(object value)
        {
            currentTable.Value.Last().Add(value);
            return this;
        }

        public TableResultBuilder AddTableToResult()
        {
            if (currentTable == null)
            {
                throw new ArgumentNullException("Table is not initialized. Cannot add null value as table.");
            }
            else
            {
                tableResult.Values.Add(currentTable);
                return this;
            }
        }
    }
}