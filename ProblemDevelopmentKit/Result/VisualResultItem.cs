using System;
using System.Collections.Generic;
using System.Linq;

namespace ProblemDevelopmentKit.Result
{
    public class VisualResultItem : IResultItem
    {
        public string Title { get; set; }
        public List<double> Keys { get; private set; }
        public List<double> Values { get; private set; }

        public VisualResultItem() : this(string.Empty, new List<double>(), new List<double>()) { }

        public VisualResultItem(string title, IEnumerable<double> keys, IEnumerable<double> values)
        {
            Title = title;

            if (keys != null && values != null)
            {
                if (isKeysAndValuesTheSameLength(keys, values))
                {
                    Keys = new List<double>(keys);
                    Values = new List<double>(values);
                }
                else
                {
                    throw new ArgumentException("Keys and values must have the same length!");
                }
            }
            else
            {
                Keys = new List<double>();
                Values = new List<double>();
            }
        }

        public void AddPoint(double key, double value)
        {
            Keys.Add(key);
            Values.Add(value);
        }

        public void RemoveItem(int index)
        {
            Keys.RemoveAt(index);
            Values.RemoveAt(index);
        }

        public double[,] GetValue()
        {
            double[,] result = new double[Keys.Count, 2];
            for (int i = 0; i < Keys.Count; ++i)
            {
                result[i, 0] = Keys[i];
                result[i, 1] = Values[i];
            }
            return result;
        }

        private bool isKeysAndValuesTheSameLength(IEnumerable<double> keys, IEnumerable<double> values)
        {
            return keys.Count() == values.Count();
        }
    }
}
