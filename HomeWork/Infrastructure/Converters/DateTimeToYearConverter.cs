using System;
using System.Globalization;
using System.Windows.Data;

namespace HomeWork.Infrastructure
{
    class DateTimeToYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.Year;
            }
            throw new Exception(); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime result = DateTime.Parse("01/01/1000");//устанавливаю дефолтное значение
            if(DateTime.TryParse("01/10/" + value.ToString(), out result))//пытаюсь преобразовать
            {
                return result;
            }
            return result;
        }
    }
}