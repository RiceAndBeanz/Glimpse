using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Gemslibe.Xamarin.Droid.UI.SwipeCards;
using Android.Util;
using Android.Support.V7.Widget;

namespace Glimpse.Droid.Controls
{
    public class CustomCardView : CardView
    {
        public CustomCardView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CustomCardView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public CustomCardView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomCardView(Context context) : base(context)
        {
        }


    }
}