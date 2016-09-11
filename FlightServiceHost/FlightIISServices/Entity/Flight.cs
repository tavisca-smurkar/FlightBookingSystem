using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace FlightIISServices.Entity
{
    [DataContract]
    public class Flight
    {
        [DataMember]
        public string FlightId { set; get; }
        [DataMember]
        public string AirlineName { set; get; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string Destination { get; set; }
        [DataMember]
        public string FlightClass { get; set; }
        [DataMember]
        public int  Price{ get; set; }
        [DataMember]
        public string ArrivalTime { get; set; }
        [DataMember]
        public string DepartureTime { get; set; }

        [DataMember]
        public int AvailableSeat { get; set; }

        [DataMember]
        public double Rating { set; get; }
    }
}