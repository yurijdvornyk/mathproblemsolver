namespace ProblemLibrary
{
    public enum ProblemResultType
    {
        /// <summary>
        /// One dimension. Use for one or more scalar values. Save the result as object[].
        /// </summary>
        D1,

        /// <summary>
        /// Two dimensions. Good solution to store x, f(x) pairs. 
        /// Save the result as object[,] where the second dimension length equals to 1
        /// </summary>
        D2,

        /// <summary>
        /// Use when you need to save matrices or data in custom format.
        /// Save the result as object[,]. You are free to fill this matrix.
        /// </summary>
        Matrix,

        /// <summary>
        /// Collection of matrices: Will be interpreted like "IEnumerable<object[,]>".
        /// </summary>
        MatrixCollection,
    }
}