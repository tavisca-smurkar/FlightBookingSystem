using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace FlightIISServices.Entity
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string FisrtName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
    }
}