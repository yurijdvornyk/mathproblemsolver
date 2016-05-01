using System;
using System.Collections.Generic;

namespace ProblemLibrary
{
    /// <summary>
    /// Stores the result of problem solution,.
    /// </summary>
    public class ProblemResult
    {
        /// <summary>
        /// Name for keys axis in visual representation.
        /// </summary>
        public string VisualTitleKey { get; private set; }

        /// <summary>
        /// Name for values axis in visual representation.
        /// </summary>
        public string VisualTitleValue { get; private set; }

        /// <summary>
        /// Some comments to solution. Use HTML markup to improve readibility.
        /// </summary>
        public string Comments { get; set; }
        
        /// <summary>
        /// Collection of result tables.
        /// </summary>
        public List<ProblemResultTableValue> TableValues { get; private set; }

        /// <summary>
        /// Collection of data for plots.
        /// </summary>
        public List<ProblemResultVisualValue> VisualValues { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProblemResult() : 
            this(string.Empty, string.Empty, new List<ProblemResultTableValue>(), new List<ProblemResultVisualValue>())
        { }

        /// <summary>
        /// ProblemResult constructor.
        /// </summary>
        /// <param name="tableValues">Collection of result tables.</param>
        /// <param name="visualValues">Collection of data for plots.</param>
        public ProblemResult(List<ProblemResultTableValue> tableValues, List<ProblemResultVisualValue> visualValues) : 
            this(string.Empty, string.Empty, tableValues, visualValues)
        { }

        /// <summary>
        /// ProblemResult constructor.
        /// </summary>
        /// <param name="visualTitleKey">Name for keys axis in visual representation.</param>
        /// <param name="visualTitleValue">Name for values axis in visual representation.</param>
        /// <param name="tableValues">Collection of result tables.</param>
        /// <param name="visualValues">Collection of data for plots.</param>
        /// <param name="comments">Some comments to solution. Use HTML markup to improve readibility.</param>
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

        /// <summary>
        /// Add table to TableValues.
        /// </summary>
        /// <param name="result"></param>
        public void AddTableResult(ProblemResultTableValue result)
        {
            TableValues.Add(result);
        }

        /// <summary>
        /// Add plot data to VisualValues.
        /// </summary>
        /// <param name="result"></param>
        public void AddVisualResult(ProblemResultVisualValue result)
        {
            VisualValues.Add(result);
        }
    }

    /// <summary>
    /// Stores the result table.
    /// </summary>
    public class ProblemResultTableValue
    {
        /// <summary>
        /// Table title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Table's columns names.
        /// </summary>
        public string[] Titles { get; private set; }

        /// <summary>
        /// Matrix of table values.
        /// </summary>
        public object[,] Values { get; private set; }

        /// <summary>
        /// Default consytuctor.
        /// </summary>
        public ProblemResultTableValue() : this(null, new string[0], new object[0,0]) { }

        /// <summary>
        /// ProblemResultTableValue constructor.
        /// </summary>
        /// <param name="titles">Array of tabl's columns names.</param>
        /// <param name="values">Matrix of table values.</param>
        public ProblemResultTableValue(string[] titles, object[,] values): this(null, titles, values) { }

        /// <summary>
        /// ProblemResultTableValue constructor.
        /// </summary>
        /// <param name="title">Table title.</param>
        /// <param name="titles">Array of tabl's columns names.</param>
        /// <param name="values">Matrix of table values.</param>
        public ProblemResultTableValue(string title, string[] titles, object[,] values)
        {
            Title = title;
            Titles = titles;
            Values = values;
        }
    }

    /// <summary>
    /// Stores data for result visual representation.
    /// </summary>
    public class ProblemResultVisualValue
    {
        /// <summary>
        /// Plot title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Axis with keys
        /// (e.g. this is the array of 'x' when you have function y = f(x) as the result).
        /// </summary>
        public double[] Keys { get; private set; }

        /// <summary>
        /// Axis with values
        /// (e.g. this is the array of 'y' when you have function y = f(x) as the result).
        /// </summary>
        public double[] Values { get; private set; }

        /// <summary>
        /// Default contructor.
        /// </summary>
        public ProblemResultVisualValue() : this(string.Empty, new double[0], new double[0]) { }

        /// <summary>
        /// ProblemResultVisualValue constructor.
        /// </summary>
        /// <param name="title">Plot title.</param>
        /// <param name="keys">Keys.</param>
        /// <param name="values">Values.</param>
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