using System;
using System.Collections.Generic;

namespace ProblemLibrary
{
    public class ProblemResult
    {
        public string VisualTitleKey { get; private set; }
        public string VisualTitleValue { get; private set; }
        public string Comments { get; set; }
        public List<ProblemResultTableValue> TableValues { get; private set; }
        public List<ProblemResultVisualValue> VisualValues { get; private set; }

        public ProblemResult() : 
            this(string.Empty, string.Empty, new List<ProblemResultTableValue>(), new List<ProblemResultVisualValue>())
        { }

        public ProblemResult(List<ProblemResultTableValue> tableValues, List<ProblemResultVisualValue> visualValues) : 
            this(string.Empty, string.Empty, tableValues, visualValues)
        { }

        public ProblemResult(
            string visualTitleKey, 
            string visualTitleValue, 
            List<ProblemResultTableValue> tableValues, 
            List<ProblemResultVisualValue> visualValues,
            string comments = "")
        {
            TableValues = tableValues;
            VisualValues = visualValues;
            VisualTitleKey = visualTitleKey;
            VisualTitleValue = visualTitleValue;
            Comments = comments;
        }
    }

    public class ProblemResultTableValue
    {
        public string Title { get; set; }
        public string[] Titles { get; private set; }
        public object[,] Values { get; private set; }

        public ProblemResultTableValue() : this(null, new string[0], new object[0,0]) { }

        public ProblemResultTableValue(string[] titles, object[,] values): this(null, titles, values) { }

        public ProblemResultTableValue(string title, string[] titles, object[,] values)
        {
            Title = title;
            Titles = titles;
            Values = values;
        }
    }

    public class ProblemResultVisualValue
    {
        public string Title { get; set; }
        public double[] Keys { get; private set; }
        public double[] Values { get; private set; }

        public ProblemResultVisualValue() : this(string.Empty, new double[0], new double[0]) { }

        public ProblemResultVisualValue(string title, double[] keys, double[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Keys and values must have the same length!");
            }
            Title = title;
            Keys = keys;
            Values = values;
        }
    }
}