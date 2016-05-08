using System.Collections.Generic;

namespace ProblemDevelopmentKit
{
    public interface IProblem
    {
        /// <summary>
        /// Problem name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Problem equation.
        /// </summary>
        string Equation { get; }

        /// <summary>
        /// Some comments to the problem. HTML tags can be used for text formatting.
        /// </summary>
        string Comments { get; }

        /// <summary>
        /// List of input parameters.
        /// </summary>
        List<ProblemDataItem> InputData { get; }

        /// <summary>
        /// Stores information about problem solution.
        /// </summary>
        ProblemResult Result { get; }

        /// <summary>
        /// True if input data is set, False in another case.
        /// </summary>
        bool IsInputDataSet { get; }

        /// <summary>
        /// True if the problem with this input data is already executed, False in another case.
        /// </summary>
        bool IsExecuted { get; }

        /// <summary>
        /// Set input argument to the InputData field and parses it using ParseData method.
        /// Then set IsInputDataSet property to True.
        /// </summary>
        /// <param name="inputData">Data to be set to InputData</param>
        void SetInputData(List<object> inputData);

        /// <summary>
        /// Parse InputData to problem parameters.
        /// </summary>
        void ParseData();

        /// <summary>
        /// Reset InputData: all parameters become null. Sets IsInputDataSet to False.
        /// </summary>
        void ResetInputData();

        /// <summary>
        /// Perform solving the problem.
        /// </summary>
        ProblemResult Execute();

        /// <summary>
        /// Wrap Calculate() method and set IsExecuted to True.
        /// </summary>
        ProblemResult Solve();
    }
}