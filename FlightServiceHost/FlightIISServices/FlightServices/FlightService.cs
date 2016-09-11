using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlightIISServices.Entity;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;

namespace FlightIISServices.FlightServices
{
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.Single)]
    public class FlightService : IFlightService
    {
        
        public List<Flight> GetFlightsBySourceDestinationTravellersAndClass(string source, string destination, int traveller, string flightClass)
        {
            List<Flight> flightList = new List<Flight>();
            XDocument doc = XDocument.Load(@"D:\FlightBookingSystem\FlightServiceHost\FlightIISServices\Data\Flights.xml");
            Console.WriteLine(doc);
            var query = from d in doc.Descendants("Flight")
                        where d.Element("Source").Value.Equals(source) && d.Element("Destination").Value.Equals(destination) && Convert.ToInt32(d.Element("Class").Element(flightClass).Element("Available").Value) >= traveller
                        select d;


            return CreateFlightList(flightClass, flightList, query);

        }

        private static List<Flight> CreateFlightList(string flightClass, List<Flight> flightList, IEnumerable<XElement> query)
        {
            foreach (var q in query)
            {
                flightList.Add(new Flight
                {
                    FlightId = q.Element("Id").Value,

                    Source = q.Element("Source").Value,
                    Destination = q.Element("Destination").Value,
                    AirlineName = q.Element("AirlineName").Value,
                    FlightClass = flightClass,
                    DepartureTime = q.Element("DepartureTime").Value,
                    ArrivalTime = q.Element("ArrivalTime").Value,
                    Price = Convert.ToInt32(q.Element("Class").Element(flightClass).Element("Price").Value),
                    Rating=Convert.ToDouble(q.Element("Rating").Value),
                    AvailableSeat = Convert.ToInt32(q.Element("Class").Element(flightClass).Element("Available").Value)
                });
            }
            return flightList;
        }

        

        public void AddNewBooking(Flight flight, Customer customer)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"D:\FlightBookingSystem\FlightServiceHost\FlightIISServices\ReferenceFiles\BookingDetails.xml");
            XmlNode Booking = xDoc.CreateElement("Booking");
            xDoc.DocumentElement.AppendChild(Booking);

            XmlNode BookingId = xDoc.CreateElement("BookingId");
            BookingId.InnerText = customer.CustomerId + "-" + flight.FlightId;
            Booking.AppendChild(BookingId);

            XmlNode CustomerIdForeignKey = xDoc.CreateElement("CustomerId");
            CustomerIdForeignKey.InnerText =customer.CustomerId;
            Booking.AppendChild(CustomerIdForeignKey);

            //XmlNode CustomerName = xDoc.CreateElement("CustomerName");
            //CustomerName.InnerText = customer.FisrtName+" "+customer.LastName ;
            //Booking.AppendChild(CustomerName);

            //XmlNode EmailId = xDoc.CreateElement("EmailId");
            //EmailId.InnerText = customer.Email;
            //Booking.AppendChild(EmailId);

            //XmlNode MobileNumber = xDoc.CreateElement("MobileNumber");
            //MobileNumber.InnerText = customer.MobileNumber;
            //Booking.AppendChild(MobileNumber);

            XmlNode FlightId = xDoc.CreateElement("FlightId");
            FlightId.InnerText = flight.FlightId;
            Booking.AppendChild(FlightId);

            XmlNode AirlineName = xDoc.CreateElement("AirlineName");
            AirlineName.InnerText = flight.AirlineName;
            Booking.AppendChild(AirlineName);

            XmlNode Source = xDoc.CreateElement("Source");
            Source.InnerText = flight.Source;
            Booking.AppendChild(Source);

            XmlNode Destination = xDoc.CreateElement("Destination");
            Destination.InnerText = flight.Destination;
            Booking.AppendChild(Destination);

            XmlNode Class = xDoc.CreateElement("Class");
            Class.InnerText = flight.FlightClass;
            Booking.AppendChild(Class);

            XmlNode Price = xDoc.CreateElement("Price");
            Price.InnerText = flight.Price.ToString();
            Booking.AppendChild(Price);

            XmlNode DepartureTime = xDoc.CreateElement("DepartureTime");
            DepartureTime.InnerText = flight.DepartureTime;
            Booking.AppendChild(DepartureTime);

            XmlNode ArrivalTime = xDoc.CreateElement("ArrivalTime");
            ArrivalTime.InnerText = flight.ArrivalTime;
            Booking.AppendChild(ArrivalTime);

            XmlNode BookingStatus = xDoc.CreateElement("BookingStatus");
            BookingStatus.InnerText = "Booked";
            Booking.AppendChild(BookingStatus);

            xDoc.DocumentElement.AppendChild(Booking);

            XmlNode Customer = xDoc.CreateElement("Customer");
            xDoc.DocumentElement.AppendChild(Customer);

            XmlNode CustomerId = xDoc.CreateElement("CustomerId");
            CustomerId.InnerText = customer.CustomerId;
            Customer.AppendChild(CustomerId);

            XmlNode CustomerName = xDoc.CreateElement("CustomerName");
            CustomerName.InnerText = customer.FisrtName + " " + customer.LastName;
            Customer.AppendChild(CustomerName);

            XmlNode EmailId = xDoc.CreateElement("EmailId");
            EmailId.InnerText = customer.Email;
            Customer.AppendChild(EmailId);

            XmlNode MobileNumber = xDoc.CreateElement("MobileNumber");
            MobileNumber.InnerText = customer.MobileNumber;
            Customer.AppendChild(MobileNumber);
            xDoc.DocumentElement.AppendChild(Customer);

            xDoc.Save(@"D:\FlightBookingSystem\FlightServiceHost\FlightIISServices\ReferenceFiles\BookingDetails.xml");
  
        }

        public List<Flight> FilteringFlights(List<Flight> flightList, Filter filter)
        {
            FilterFlights filterFlight = new FilterFlights();
            if (filter.AirlineName != null)
            {
                flightList = filterFlight.FilterListByAirlineName(flightList, filter.AirlineName);
            }
            if(filter.Rating != 0)
            {
                flightList = filterFlight.FilterListByRating(flightList, filter.Rating);
            }
            if(filter.StartRange!=0 && filter.EndRange != 0)
            {
                flightList = filterFlight.FilterListByPrice(flightList, filter.StartRange, filter.EndRange);
            }
            //throw new NotImplementedException();
            return flightList;
        }
    }
}