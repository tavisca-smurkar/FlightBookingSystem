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
        Result GetFlightsBySourceDestinationTravellersAndClass(string source,string destination, string traveller, string flightClass);
        [OperationContract]
        Result FilteringFlights(Result result,Filter filter);
        [OperationContract]
        string AddNewBooking(Flight flight, Customer customer, int travellers);
        [OperationContract]
        string CancelBooking(string bookindId);
        [OperationContract]
        Result SaveCardDetails(Result result,Card card);
    }
}
