using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Glimpse.Core.Converters
{
    public class DateTimeConverter : MvxValueConverter<DateTime, string>
    {

        protected override DateTime ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Parse(value); ; 
        }
    }
}
