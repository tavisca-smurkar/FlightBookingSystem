using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlightIISServices.Entity;

namespace FlightIISServices.FlightServices
{
    public class FilterFlights
    {
        public List<Flight> FilterListByPrice(List<Flight> flightList, int rangeFrom, int rangeUpto)
        {
            var query = from d in flightList where d.Price >= rangeFrom && d.Price <= rangeUpto select d;
            return query.ToList();
        }

        public List<Flight> FilterListByAirlineName(List<Flight> flightList, string airlineName)
        {
            var query = from d in flightList where d.AirlineName.Equals(airlineName) select d;
            return query.ToList();
        }

        public List<Flight> FilterListByRating(List<Flight> flightList, double rating)
        {
            var query = from d in flightList where d.Rating >= rating select d;
            return query.ToList();
        }
    }
}