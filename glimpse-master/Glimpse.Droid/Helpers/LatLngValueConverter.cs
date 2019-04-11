using System;
using System.Globalization;
using Android.Gms.Maps.Model;
using Glimpse.Core.Model;
using MvvmCross.Platform.Converters;

namespace Glimpse.Droid.Helpers
{
    public class LatLngValueConverter : MvxValueConverter<Location, LatLng>
    {
        protected override LatLng Convert(Location value, Type targetType, object parameter, CultureInfo culture)
        {
            return new LatLng(value.Lat, value.Lng);
        }

        protected override Location ConvertBack(LatLng value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Location {Lat = value.Latitude, Lng = value.Longitude};
        }
    }
}