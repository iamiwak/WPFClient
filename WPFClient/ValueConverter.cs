using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFClient
{
    public class ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            //if (targetType != typeof(DateTime))
            //    return null;

            string dts;
            try
            {
                DateTime dt = (DateTime)value;
                dts = dt.ToString("dd.MM.yyyy");
            } catch (InvalidCastException)
            {
                string[] dates = value.ToString().Split('.');
                int year = int.Parse(dates[2].Split(' ')[0]);
                int month = int.Parse(dates[1]);
                int day = int.Parse(dates[0]);
                DateTime dt = new DateTime(year, month, day);
                dts = dt.ToString("dd.MM.yyyy");
            }
            return dts;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
}
