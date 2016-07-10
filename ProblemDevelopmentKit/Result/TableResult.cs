using System.Collections.Generic;

namespace ProblemDevelopmentKit.Result
{
    public class TableResult : IResult
    {
        public string Title { get; set; }
        public List<TableResultItem> Values { get; private set; }

        public TableResult() : this("") { }

        public TableResult(string title)
        {
            Title = title;
            Values = new List<TableResultItem>();
        }
    }
}