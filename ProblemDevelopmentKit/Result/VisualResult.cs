using System.Collections.Generic;

namespace ProblemDevelopmentKit.Result
{
    public class VisualResult : IResult
    {
        public string Title { get; set; }
        public string KeyTitle { get; set; }
        public string ValueTitle { get; set; }
        public List<VisualResultItem> Graphs { get; private set; }

        public VisualResult()
        {
            Graphs = new List<VisualResultItem>();
        }

        public VisualResult(string title, string keyTitle, string valueTitle)
        {
            Title = title;
            KeyTitle = keyTitle;
            ValueTitle = valueTitle;
        }
    }
}