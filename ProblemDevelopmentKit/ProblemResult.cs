using ProblemDevelopmentKit.Result;
using System;
using System.Collections.Generic;

namespace ProblemDevelopmentKit
{
    /// <summary>
    /// Stores the result of problem solution.
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
        public TableResult TableResult { get; private set; }

        /// <summary>
        /// Collection of data for plots.
        /// </summary>
        public VisualResult VisualResult { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProblemResult() : 
            this(string.Empty, string.Empty, new TableResult(), new VisualResult())
        { }

        public ProblemResult(TableResult tableValues, VisualResult visualValues) : 
            this(string.Empty, string.Empty, tableValues, visualValues)
        { }

        public ProblemResult(
            string visualTitleKey,
            string visualTitleValue,
            TableResult tableValues,
            VisualResult visualValues,
            string comments = "")
        {
            TableResult = tableValues;
            VisualResult = visualValues;
            VisualTitleKey = visualTitleKey;
            VisualTitleValue = visualTitleValue;
            Comments = comments;
        }
    }
}