using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FlightIISServices.Entity
{
    [DataContract]
    public class Card
    {
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public string validTillMonthAndYear { get; set; }
        [DataMember]
        public int CVV { get; set; }
        [DataMember]
        public string CardHolderName { get; set; }
    }
}