using System;
using System.Globalization;
using Android.Gms.Maps.Model;
using Glimpse.Core.Model;
using MvvmCross.Platform.Converters;
using Android.Graphics;

namespace Glimpse.Droid.Helpers
{
    public class ByteArrayImageValueConverter : MvxValueConverter<byte[], Bitmap>
    {
        protected override Bitmap Convert(byte[] value, Type targetType, object parameter, CultureInfo culture)
        {
            return BitmapFactory.DecodeByteArray(value, 0, value.Length);
        }

     /*   protected override Location ConvertBack(LatLng value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Location {Lat = value.Latitude, Lng = value.Longitude};
        }*/
    }
}