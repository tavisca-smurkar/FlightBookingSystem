﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightIISServices;

namespace FlightIIsServiceTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            FlightIISServices.FlightServices.FlightService flightService = new FlightIISServices.FlightServices.FlightService();
            var a = flightService.GetFlightsBySourceDestinationTravellersAndClass("Pune", "Mumbai", 2, "Economy");

            var b = flightService.FilteringFlights(a, new FlightIISServices.Entity.Filter
            {
                //Rating = 3.5,
                //AirlineName="Vistara",
                StartRange = 2500,
                EndRange=3500,
                
            });
            
            flightService.AddNewBooking(b[0], new FlightIISServices.Entity.Customer { CustomerId = "1",FisrtName="Mayuresh",LastName="Bhanushali",Email="mb@tv.com",MobileNumber="9854659887" });

        }
        
    }
}
