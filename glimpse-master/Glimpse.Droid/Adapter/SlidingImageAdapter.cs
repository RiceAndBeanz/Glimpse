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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Java.Lang;
using Android.Graphics;

namespace Glimpse.Droid.Adapter
{
    public class SlidingImageAdapter : PagerAdapter
    {

        Context _context;
        List<Bitmap> _resources;
        LayoutInflater _layoutInflater;

        public SlidingImageAdapter(Context context, List<Bitmap> resources)
        {
            _context = context;
            _resources = resources;
        }


        public override int Count
        {
            get
            {
                return _resources.Count;
            }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == ((LinearLayout)@object);
  
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            _layoutInflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            View itemView = _layoutInflater.Inflate(Resource.Layout.ImageSwipeLayout, container, false);
            ImageView imageView = (ImageView)itemView.FindViewById(Resource.Id.pagerImageItem);
            imageView.SetImageBitmap(_resources[position]);
            container.AddView(itemView);

            return itemView;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            container.RemoveView((LinearLayout)@object);
        }

    }
}