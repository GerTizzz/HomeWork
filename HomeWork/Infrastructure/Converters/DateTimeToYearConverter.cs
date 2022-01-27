using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HomeWork.Infrastructure
{
    class DateTimeToYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals((DateTime)value, new DateTime()))
            {
                return null;
            }
            else
                return ((DateTime)value).ToString("yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}