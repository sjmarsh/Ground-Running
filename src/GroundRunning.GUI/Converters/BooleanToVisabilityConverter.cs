using System;
using System.Windows.Data;

namespace GroundRunning.GUI.Converters
{
    public class BooleanToVisabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var boolValue = value as Boolean?;

            return boolValue.HasValue && boolValue.Value == true ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
