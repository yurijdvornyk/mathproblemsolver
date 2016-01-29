using System.Collections.Generic;

namespace ProblemLibrary
{
    public interface IProblem
    {
        /// <summary>
        /// Problem name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Problem equation written in LATEX form.
        /// </summary>
        string Equation { get; }

        /// <summary>
        /// List of input parameters.
        /// </summary>
        List<ProblemDataItem> InputData { get; }

        /// <summary>
        /// Result of calculating the problem.
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
        /// Sets input argument to the InputData field and parses it using ParseData method.
        /// Then sets IsInputDataSet property to True
        /// </summary>
        /// <param name="inputData">Data to be set to InputData</param>
        void SetInputData(List<object> inputData);

        /// <summary>
        /// Parses InputData to problem parameters.
        /// </summary>
        void ParseData();

        /// <summary>
        /// Resets InputData: all parameters become null. Sets IsInputDataSet to False.
        /// </summary>
        void ResetInputData();

        /// <summary>
        /// Performs solving the problem.
        /// </summary>
        ProblemResult Execute();

        /// <summary>
        /// Sets Calculate() result to Result property and sets IsExecuted to True.
        /// </summary>
        ProblemResult Solve();
    }
}