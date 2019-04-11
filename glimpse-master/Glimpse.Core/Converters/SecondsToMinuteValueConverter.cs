using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Converters
{
    public class SecondsToMinuteValueConverter : MvxValueConverter<int, string>
    {
        protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timespan = TimeSpan.FromSeconds(value);
            int totalMins = (int)timespan.TotalMinutes;
            string displayTime;
            if (Glimpse.Core.Services.General.Settings.Language == "Français")
            {
                displayTime ="Le temps de conduite à la promotion : "+ totalMins;
            }
            else
                displayTime = "Driving time to promotion : " + totalMins;

            if (totalMins == 1)
                displayTime = displayTime + " minute";
            else
                displayTime = displayTime + " minutes";

            return displayTime;
        }
    }
}
