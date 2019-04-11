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
using Android.Support.V4.View;
using Android.Util;

namespace Glimpse.Droid.Controls
{
    public class CustomViewPager: ViewPager
    {
        private bool _swipeable = true;

        public CustomViewPager(Context context):base(context){ }

        public CustomViewPager(Context context, IAttributeSet attrs) : base(context, attrs) { }

        // Call this method in your motion events when you want to disable or enable
        // It should work as desired.
        public void SetSwipeable(bool swipeable)
        {
            _swipeable = swipeable;
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return (_swipeable)?base.OnInterceptTouchEvent(ev) : false ;
        }

    }
}