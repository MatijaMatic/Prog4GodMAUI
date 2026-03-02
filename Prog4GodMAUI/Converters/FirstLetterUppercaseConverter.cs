using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Prog4GodMAUI.Converters
{
    public class FirstLetterUppercaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
