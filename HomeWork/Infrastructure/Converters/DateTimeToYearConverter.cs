using System;
using System.Globalization;
using System.Windows.Data;

namespace HomeWork.Infrastructure
{
    class DateTimeToYearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals((DateTime)value, new DateTime()))//проверяю не пустое ли значение
            {
                return null;
            }
            else
                return int.Parse(((DateTime)value).ToString("yyyy"));//возвращаю только год из всей временной даты
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