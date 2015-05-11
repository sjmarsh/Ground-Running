using System;
using System.Windows.Data;

namespace GroundRunning.GUI.Converters
{
    public class BooleanToVisabilityConverter : IValueConverter
    {
        public bool Invert { get; set; } 

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var boolValue = value as Boolean?;
            var isVisible = boolValue.HasValue && boolValue.Value == true;
            
            if(Invert)
            {
                isVisible = !isVisible;
            }

            return isVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
