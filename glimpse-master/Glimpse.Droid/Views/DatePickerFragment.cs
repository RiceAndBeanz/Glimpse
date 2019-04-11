using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Globalization;

namespace Glimpse.Droid.Views
{
    public class DatePickerFragment : DialogFragment,
                                   DatePickerDialog.IOnDateSetListener
    {       

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(DateTime minDate, Action<DateTime> onDateSelected)
        {
            Bundle bundle = new Bundle();
            bundle.PutLong("minDate", minDate.Ticks);
            DatePickerFragment frag = new DatePickerFragment();            
            frag._dateSelectedHandler = onDateSelected;
            frag.Arguments = bundle;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);

            DateTime minDate= new DateTime(Arguments.GetLong("minDate"));
            dialog.DatePicker.MinDate = SetMinDate(minDate);
            return dialog;
        }

        public static long SetMinDate(DateTime minDate)
        {            
            DateTime start = new DateTime(1970, 1, 1);
            TimeSpan ts = (minDate - start);

            //Add Days to SetMax Days;
            int noOfDays = ts.Days + 1;
            return (long)(TimeSpan.FromDays(noOfDays).TotalMilliseconds);
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _dateSelectedHandler(selectedDate);
        }
    }
}