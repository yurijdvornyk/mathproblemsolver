namespace ProblemLibrary
{
    public enum ProblemResultType
    {
        /// <summary>
        /// One or more scalar values: one-dimensional or multi-dimensional array.
        /// Will be interpreted like "object[,]".
        /// </summary>
        Matrix,

        /// <summary>
        /// Collection of matrices: Will be interpreted like "IEnumerable<object[,]>".
        /// </summary>
        MatrixCollection
    }
}