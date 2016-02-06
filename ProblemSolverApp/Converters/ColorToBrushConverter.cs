using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ProblemSolverApp.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Media.Color)
            {
                return new SolidColorBrush((System.Windows.Media.Color)value);
            }

            if (value is System.Drawing.Color)
            {
                var color = (System.Drawing.Color)value;
                System.Windows.Media.Color trueColor = new Color();
                trueColor.R = color.R;
                trueColor.G = color.G;
                trueColor.B = color.B;
                trueColor.A = color.A;
                return new SolidColorBrush(trueColor);
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
