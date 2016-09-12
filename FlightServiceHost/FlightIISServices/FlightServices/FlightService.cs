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
        string FlightXMLPath = @"..\..\..\FlightIISServices\Data\Flights.xml";
        string BookingDetailsXMLPath = @"..\..\..\FlightIISServices\Data\BookingDetails.xml";
        string cardDetailsxmlPath= @"..\..\..\FlightIISServices\Data\Cards.xml";
        public Result GetFlightsBySourceDestinationTravellersAndClass(string source, string destination, string traveller, string flightClass)
        {
            Result result = new Result();
            try
            {
               if (!FlightIISServices.Validations.Validator.ValidatePositiveNumberGreaterThanZero(traveller))
                {
                    throw new Exception("Enter valid number of travellers. Traveller's number should be atleast 1 and number");
                }

                List<Flight> flightList = new List<Flight>();
                XDocument doc = XDocument.Load(FlightXMLPath);
               
                var query = from d in doc.Descendants("Flight")
                            where d.Element("Source").Value.Equals(source) && d.Element("Destination").Value.Equals(destination) && Convert.ToInt32(d.Element("Class").Element(flightClass).Element("Available").Value) >= Convert.ToInt32(traveller)
                            select d;
                if(query==null)
                {
                    throw new Exception("No result found.");
                }
                result.Status = true;
                result.Message = "Flight List retrive successfully !";
                result.FlightList = CreateFlightList(flightClass, flightList, query);
                return result;
                
            }
            catch (Exception ae)
            {
                result.Status = false;
                result.Message = ae.Message;
                result.FlightList = null;
                return result;
            }
            
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

        

        public string AddNewBooking(Flight flight, Customer customer)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(BookingDetailsXMLPath);
            XmlNode Booking = xDoc.CreateElement("Booking");
            xDoc.DocumentElement.AppendChild(Booking);

            XmlNode BookingId = xDoc.CreateElement("BookingId");
            BookingId.InnerText = customer.CustomerId + "-" + flight.FlightId;
            Booking.AppendChild(BookingId);

            XmlNode CustomerIdForeignKey = xDoc.CreateElement("CustomerId");
            CustomerIdForeignKey.InnerText =customer.CustomerId;
            Booking.AppendChild(CustomerIdForeignKey);

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

            xDoc.Save(BookingDetailsXMLPath);
            return customer.CustomerId + "-" + flight.FlightId;
        }

        public Result FilteringFlights(Result result, Filter filter)
        {
            try
            {
                FilterFlights filterFlight = new FilterFlights();
                if (filter.AirlineName != null)
                {
                    result.FlightList = filterFlight.FilterListByAirlineName(result.FlightList, filter.AirlineName);
                }
                if (filter.Rating != 0)
                {
                    result.FlightList = filterFlight.FilterListByRating(result.FlightList, filter.Rating);
                }
                if (filter.StartRange != 0 && filter.EndRange != 0)
                {
                    result.FlightList = filterFlight.FilterListByPrice(result.FlightList, filter.StartRange, filter.EndRange);
                }

                if(result.FlightList.Count==0) throw new Exception("No result found.");
                return result;
            }
            catch(Exception ae)
            {
                result.Status = false;
                result.Message = ae.Message;
                result.FlightList = null;
                return result;
            }
        }

        public string CancelBooking(string bookindId)
        {
            XDocument doc = XDocument.Load(BookingDetailsXMLPath);
            var a = doc.Descendants("Booking").First(x=>x.Element("BookingId").Value.Equals(bookindId));
            a.Element("BookingStatus").Value = "Cancelled";
            doc.Save(BookingDetailsXMLPath);
            return bookindId;
        }

        public Result SaveCardDetails(Result result, Card card)
        {
            try
            {
                if (!Validations.Validator.ValidateCardNumber(card.CardNumber))
                {
                    throw new Exception("Card details is not valid. Please enter valid card number");
                }
                else if (!Validations.Validator.ValidateCVVNumber(card.CVV.ToString()))
                {
                    throw new Exception("Card CVV code is not valid. Please enter valid CVV code.");
                }
                else if (!Validations.Validator.ValidateName(card.CardHolderName))
                {
                    throw new Exception("Card holder's name is not valid. Please enter valid holder name.");
                }
                else
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(cardDetailsxmlPath);
                    XmlNode newCard = document.CreateElement("Card");
                    document.DocumentElement.AppendChild(newCard);

                    XmlNode cardNumber = document.CreateElement("CardNumer");
                    cardNumber.InnerText = card.CardNumber;
                    newCard.AppendChild(cardNumber);

                    XmlNode validTillMonthAndYear = document.CreateElement("ValidTillMonthAndYear");
                    validTillMonthAndYear.InnerText = card.validTillMonthAndYear;
                    newCard.AppendChild(validTillMonthAndYear);

                    XmlNode cVV = document.CreateElement("CVV");
                    cVV.InnerText = card.CVV.ToString();
                    newCard.AppendChild(cVV);

                    XmlNode cardHolderName = document.CreateElement("CardHolderName");
                    cardHolderName.InnerText = card.CardHolderName;
                    newCard.AppendChild(cardHolderName);
                    document.Save(cardDetailsxmlPath);

                    return result;

                }
              
            }
            catch (Exception ae)
            {
                result.Status = false;
                result.Message = ae.Message;
                result.FlightList = result.FlightList;
                return result;
            }
        }
    }
}