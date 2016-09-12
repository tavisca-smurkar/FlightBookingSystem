using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FlightIISServices.Entity
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public List<Flight> FlightList { get; set; }
    }
}