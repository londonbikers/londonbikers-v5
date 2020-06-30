using System;
using System.Configuration;
using System.Web;
using System.Net;

namespace Apollo.Utilities.GeoCoding
{
    public interface ISpatialCoordinate
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
    }

    /// <summary>
    /// Coordiate structure. Holds Latitude and Longitude.
    /// </summary>
    public struct Coordinate : ISpatialCoordinate
    {
        private double _latitude;
        private double _longitude;

        public Coordinate(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        #region ISpatialCoordinate Members
        public double Latitude { get { return _latitude; } set { _latitude = value; } }
        public double Longitude { get { return _longitude; } set { _longitude = value; } }
        #endregion
    }

    public class Geocode
    {
        private const string GoogleUri = "http://maps.google.com/maps/geo?q=";
        private static readonly string GoogleKey = ConfigurationManager.AppSettings["Tetron.GoogleMapsKey"];
        private const string OutputType = "csv"; // Available options: csv, xml, kml, json

        private static Uri GetGeocodeUri(string address)
        {
            address = HttpUtility.UrlEncode(address);
            return new Uri(String.Format("{0}{1}&output={2}&key={3}", GoogleUri, address, OutputType, GoogleKey));
        }

        /// <summary>
        /// Gets a Coordinate from a address.
        /// </summary>
        /// <param name="address">An address.
        ///     <remarks>
        ///         <example>1600 Amphitheatre Parkway Mountain View, CA 94043</example>
        ///     </remarks>
        /// </param>
        /// <returns>A spatial coordinate that contains the latitude and longitude of the address.</returns>
        public static Coordinate GetCoordinates(string address)
        {
            using (var client = new WebClient())
            {
                var uri = GetGeocodeUri(address);

                // the first number is the status code, 
                // the second is the accuracy, 
                // the third is the latitude, 
                // the fourth one is the longitude.

                var geocodeInfo = client.DownloadString(uri).Split(',');
                return new Coordinate(Convert.ToDouble(geocodeInfo[2]), Convert.ToDouble(geocodeInfo[3]));
            }
        }
    }
}