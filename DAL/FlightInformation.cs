using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FlightInformation
    {
        public String Origin { get; set; }
        public String Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int NumberOfItinerariesInbound { get; set; }
        public int NumberOfItinerariesOutbound { get; set; }
        public String NumberOfPassengers { get; set; }
        public String Currency { get; set; }
        public double TotalPrice { get; set; }

    }
}
