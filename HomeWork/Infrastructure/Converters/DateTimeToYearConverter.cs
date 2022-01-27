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
            DateTime result = DateTime.Parse("01/01/1000");
            if(DateTime.TryParse("01/10/" + value.ToString(), out result))
            {
                return result;
            }
            return result;
        }
    }
}