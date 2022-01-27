using System;
using System.Globalization;
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
                return int.Parse(((DateTime)value).ToString("yyyy"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse("01/10/" + value.ToString());
        }
    }
}