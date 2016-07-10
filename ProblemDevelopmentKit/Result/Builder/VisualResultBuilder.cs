using System;

namespace ProblemDevelopmentKit.Result.Builder
{
    public class VisualResultBuilder : IResultBuilder
    {
        private VisualResult visualResult;
        private VisualResultItem currentResult;

        private VisualResultBuilder() : this(string.Empty, string.Empty, string.Empty) { }

        private VisualResultBuilder(string title, string keyTitle, string valueTitle)
        {
            visualResult = new VisualResult(title, keyTitle, valueTitle);
        }

        public static VisualResultBuilder Create(string title = "", string keyTitle = "", string valueTitle = "")
        {
            return new VisualResultBuilder(title, keyTitle, valueTitle);
        }

        public VisualResultBuilder SetTableResult(VisualResult visualResult)
        {
            this.visualResult = visualResult;
            return this;
        }

        public VisualResult Build()
        {
            return visualResult;
        }

        public VisualResultBuilder NewGraph(string title = "")
        {
            currentResult = new VisualResultItem();
            currentResult.Title = title;
            return this;
        }

        public VisualResultBuilder AddValue(double key, double value)
        {
            currentResult.AddPoint(key, value);
            return this;
        }

        public VisualResultBuilder AddGraphToResult()
        {
            if (currentResult == null)
            {
                throw new ArgumentNullException("Table is not initialized. Cannot add null value as table.");
            }
            else
            {
                visualResult.Graphs.Add(currentResult);
                return this;
            }
        }
    }
}
