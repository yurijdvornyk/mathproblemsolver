namespace ProblemLibrary
{
    /// <summary>
    /// Stores the information about the result of solved mathematical problem.
    /// 
    /// If the result of your problem is a NUMBER, VECTOR (ARRAY) or MATRIX, you should save it like this:
    /// Type:   ProblemResultType.Matrix;
    /// Title:  string[];   e.g.: { column1, column2, column3, ... };
    /// Value:  object[,];  e.g.: { { 1, 2, 3, ... }, { 4, 5, 6, ...}, {7, 8, 9, ...}, ... };
    /// 
    /// If the result is MULTIPLE NUMBERS, VECTORS (ARRAYS) or MATRICES, use this way:
    /// Type:   ProblemResultType.MatrixCollection;
    /// Title:  IEnumerable<object[]>;
    /// Value:  IEnumerable<string[,]>;
    /// </summary>
    public class ProblemResult
    {
        public ProblemResultType Type { get; set; }
        public object Title { get; set; }
        public object Value { get; set; }

        public ProblemResult(ProblemResultType type, object title, object value)
        {            
            Type = type;
            Title = title;
            Value = value;
        }
    }
}