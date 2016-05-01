namespace ProblemLibrary
{
    /// <summary>
    /// Stores information about problem parameter.
    /// </summary>
    public class ProblemDataItem
    {
        /// <summary>
        /// Parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parameter type.
        /// </summary>
        public ProblemDataItemType Type { get; set; }

        /// <summary>
        /// Parameter's default value (optional; will be used if value is not specified).
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Parameter's value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Some extra data.
        /// </summary>
        public object ExtraData { get; set; }

        /// <summary>
        /// "true" if this parameter is required, otherwise "false".
        /// </summary>
        public bool IsRequired { get; set; }

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProblemDataItem() { }

        /// <summary>
        /// ProblemDataItem constructor.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter tyoe.</param>
        /// <param name="isRequired">"true" if parameter must be set, otherwise "false".</param>
        public ProblemDataItem(string name, ProblemDataItemType type, bool isRequired = true) : this(name, type, null, true, null) { }

        /// <summary>
        /// ProblemDataItem constructor.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="type">Parameter tyoe.</param>
        /// <param name="defaultValue">Parameter's default value (optional; will be used if value is not specified).</param>
        /// <param name="isRequired">"true" if parameter must be set, otherwise "false".</param>
        /// <param name="extraData">Some extra data.</param>
        public ProblemDataItem(string name, ProblemDataItemType type, object defaultValue, bool isRequired = true, object extraData = null)
        {
            Name = name;
            Type = type;
            DefaultValue = defaultValue;
            ExtraData = extraData;
            IsRequired = isRequired;
        }

        #endregion

        /// <summary>
        /// Set parameter's value.
        /// </summary>
        /// <param name="value">Value to be set.</param>
        public void SetValue(object value) { Value = value; }
    }
}