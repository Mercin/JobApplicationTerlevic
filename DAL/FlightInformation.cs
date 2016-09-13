using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FlightInformation
    {
        private String Origin { get; set; }
        private String Destination { get; set; }
        private String DepartureDate { get; set; }
        private String ArrivalDate { get; set; }
        private int NumberOfItinerariesInbound { get; set; }
        private int NumberOfItinerariesOutbound { get; set; }
        private int NumberOfPassengers { get; set; }
        private String Currency { get; set; }
        private double TotalPrice { get; set; }



    }
}
