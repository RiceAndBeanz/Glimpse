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

namespace Glimpse.Droid.Controls.Listener
{
    public class MySearchViewOnCloseListener : Java.Lang.Object, SearchView.IOnCloseListener
    {
        public View view;
        public bool OnClose()
        {
            view.Visibility = ViewStates.Gone;
            return false;
        }
    }
}