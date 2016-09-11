using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace FlightIISServices.Entity
{
    [DataContract]
    public class Filter
    {
        [DataMember]
        public int StartRange { set; get; }
        [DataMember]
        public int EndRange { get; set; }
        [DataMember]
        public string AirlineName { get; set; }
        [DataMember]
        public double Rating { set; get; }
    }
}