using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace S3.Converters
{
   public class DateTimeConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = new DateTime(2012, 01, 01);
            var cas = TimeSpan.FromMinutes(System.Convert.ToDouble(value));
            dt = dt + cas;
            return dt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((DateTime)value).Hour  * 60 )+ ((DateTime)value).Minute;
        }
    }
}
