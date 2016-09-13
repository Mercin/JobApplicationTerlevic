using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobApplicationTerlevic
{
    public class Helper
    {
        public static bool ValidateSearch(String originAirport, String destAirport,
                                   DateTime departureDate, DateTime returnDate,
                                   String noOfPassengers, String currency)
        {

            //Validation of origin airport IATA

            if(originAirport.Length != 3)
                {
                    MessageBox.Show("IATA code for origin needs to have 3 letters", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if(!Regex.IsMatch(originAirport, @"^[a-zA-Z]+$"))
                {
                    MessageBox.Show("IATA code for origin needs to contain only errors", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            

            //Validation of destination airport IATA

                if (destAirport.Length != 3)
                {
                    MessageBox.Show("IATA code for destination needs to have 3 letters", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (!Regex.IsMatch(destAirport, @"^[a-zA-Z]+$"))
                {
                    MessageBox.Show("IATA code for destination needs to contain only letters", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            

            //Validation if return is before departure

            if(returnDate.Date.CompareTo(departureDate.Date) < 0)
            {
                MessageBox.Show("Return date cannot be before the departure", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int intNoOfPassengers;
            //Validation if number of passengers is a number
            if (!Int32.TryParse(noOfPassengers, out intNoOfPassengers) && noOfPassengers != "" && noOfPassengers != null)
            {
                MessageBox.Show("Number of passengers needs to be a whole number", "Validation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
