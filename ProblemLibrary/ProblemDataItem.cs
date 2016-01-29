namespace ProblemLibrary
{
    public class ProblemDataItem
    {
        public string Name { get; set; }
        public ProblemDataItemType Type { get; set; }
        public object DefaultValue { get; set; }
        public object Value { get; set; }
        public object ExtraData { get; set; }
        public bool IsRequired { get; set; }

        #region Constructors

        public ProblemDataItem() { }

        public ProblemDataItem(string name, ProblemDataItemType type, bool isRequired = true) : this(name, type, null, true, null) { }

        public ProblemDataItem(string name, ProblemDataItemType type, object defaultValue, bool isRequired = true, object extraData = null)
        {
            Name = name;
            Type = type;
            DefaultValue = defaultValue;
            ExtraData = extraData;
            IsRequired = isRequired;
        }

        #endregion

        public void SetValue(object value) { Value = value; }
    }
}