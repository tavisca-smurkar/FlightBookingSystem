using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightIISServices.Entity;
using System.ServiceModel;

namespace FlightIISServices.FlightServices
{
    [ServiceContract]
    public interface IFlightService
    {
        [OperationContract]
        List<Flight> GetFlightsBySourceDestinationTravellersAndClass(string source,string destination,int traveller, string flightClass);
        [OperationContract]
        List<Flight> FilteringFlights(List<Flight> flightList,Filter filter);
        [OperationContract]
        void AddNewBooking(Flight flight, Customer customer);
       // [OperationContract]
        //void CancelBooking(string bookindId);
    }
}
