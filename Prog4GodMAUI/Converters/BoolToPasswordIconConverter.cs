using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Prog4GodMAUI.Converters
{
    public class BoolToPasswordIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "eye_off.svg" : "eye_show.svg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
