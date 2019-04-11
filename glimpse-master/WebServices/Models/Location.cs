namespace WebServices.Models
{
    public class Location
    {

        public Location()
        {
        }

        public Location(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

        public double Lat { get; set; }      

        public double Lng { get; set; }      


    }
}