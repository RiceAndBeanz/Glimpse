using System;
using CoreGraphics;
using Factorymind.Components;
using Glimpse.iOS.Utility;
using UIKit;

namespace Glimpse.iOS.Controls
{
    public static class CalendarHelper
    {
        public static FMCalendar Calendar;

        public static FMCalendar GetPreconfiguredInstance(
            CGRect frame, Action<DateTime> dateSelected)
        {
            Calendar = new FMCalendar(frame)
            {
                LeftArrow = UIImage.FromBundle("arrow-left.png"),
                RightArrow = UIImage.FromBundle("arrow-right.png"),
                SelectionColor = GlimpseColors.AccentColor,
                TodayCircleColor = GlimpseColors.AccentColor,
                MonthFormatString = "MMMM yyyy",
                SundayFirst = true,
                DateSelected = date =>
                {
                    DeselectUnavailableDate(date);
                    dateSelected?.Invoke(date);
                },
                IsDateAvailable = date => date >= DateTime.Today
            };

            return Calendar;
        }

        static void DeselectUnavailableDate(DateTime date)
        {
            if (!Calendar.IsDateAvailable(date))
            {
                Calendar.DeselectDate();
            }
        }
    }
}