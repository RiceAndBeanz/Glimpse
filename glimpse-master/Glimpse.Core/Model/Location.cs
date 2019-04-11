using System;
using MvvmCross.Core.ViewModels;
using SQLite.Net;

namespace Glimpse.Core.Model
{
    public class Location : MvxNotifyPropertyChanged, ISerializable<string>
    {
        public Location()
        {

        }

        public Location(string serializedData) : this()
        {
            var stringVals = serializedData.Split(',');
            Lat = Convert.ToDouble(stringVals[0]);
            Lng = Convert.ToDouble(stringVals[1]);
        }

        public Location(double lat, double lng)
        {
            _lat = lat;
            _lng = lng;
        }

        private double _lat;

        public double Lat
        {
            get { return _lat; }
            set
            {
                _lat = value;
                RaisePropertyChanged(() => Lat);
            }
        }

        private double _lng;

        public double Lng
        {
            get { return _lng; }
            set
            {
                _lng = value;
                RaisePropertyChanged(() => Lng);
            }
        }

        public override bool Equals(object obj)
        {
            var lRhs = obj as Location;
            if (lRhs == null)
                return false;

            return (lRhs.Lat == Lat) && (lRhs.Lng == Lng);
        }

        public override int GetHashCode()
        {
            return Lat.GetHashCode() + Lng.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", Lat, Lng);
        }

        public string Serialize()
        {
            return ToString();
        }
    }
}