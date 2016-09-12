using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
namespace FlightIISServices.Validations
{
    public class Validator
    {
        public static bool ValidateCardNumber(string cardNumber)
        {
            Regex regex = new Regex(@"^[0-9]{16}$");
            Match match = regex.Match(cardNumber);
            return match.Success;
        }
        public static bool ValidatePositiveNumberGreaterThanZero(string number)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            Match match = regex.Match(number);
            return match.Success;
        }
        public static bool ValidateName(string name)
        {
            Regex regex = new Regex(@"^[a-z A-Z]+$");
            Match match = regex.Match(name);
            return match.Success;
        }
        public static bool ValidateMobileNumber(string mobileNumber)
        {
            Regex regex = new Regex(@"^[0-9]{10}$");
            Match match = regex.Match(mobileNumber);
            return match.Success;
        }
        public static bool ValidateCVVNumber(string CVV)
        {
            Regex regex = new Regex(@"^[0-9]{3}$");
            Match match = regex.Match(CVV);
            return match.Success;
        }


    }
}