namespace ProblemLibrary
{
    public enum ProblemDataItemType
    {
        /// <summary>
        /// 12, 5, -255, 37, -44, ...
        /// </summary>
        Int,

        /// <summary>
        /// 0, 1, 2, 3, etc.
        /// </summary>
        UnsignedInt,

        /// <summary>
        /// 20.2, 18.5, -20.1903225, ...
        /// </summary>
        Double,

        /// <summary>
        /// 2+3i, -8.41+12i, ...
        /// </summary>
        Complex,

        /// <summary>
        /// a, d, $, %, 8, |, etc.
        /// </summary>
        Character,

        /// <summary>
        /// Any text
        /// </summary>
        String,

        /// <summary>
        /// List of values with one possible choice
        /// </summary>
        OneOfMany,

        /// <summary>
        /// List of values with arbitrary number of possible choices
        /// </summary>
        SomeOfMany,

        /// <summary>
        /// 1.01.2016, 2015.01.12, etc.
        /// </summary>
        Date,

        /// <summary>
        /// 15:55
        /// </summary>
        Time,

        /// <summary>
        /// 1.01.2016 10:20
        /// </summary>
        DateAndTime,

        /// <summary>
        /// True or False
        /// </summary>
        Boolean,

        /// <summary>
        /// C:\Program Files\Abc\File.xml etc.
        /// Provides dialog window to select file
        /// </summary>
        File,

        /// <summary>
        /// C:\Program Files\
        /// Provides dialog window to select folder
        /// </summary>
        Directory
    }
}