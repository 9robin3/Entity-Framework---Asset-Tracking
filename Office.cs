using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject_02_EF_AssetTracking
{
    class Office
    {
        public Office()
        {

        }

        public Office(string location)
        {
            Location = location;
            //GetRegion(price);
        }

        public void GetRegion(double price)
        {
            double toSEK = 8.67;
            double toUK = 0.72;

            if (Location.ToUpper().Trim() == "LONDON")
            {
                ConvertedPrice = price * toUK;
                Currency = "£";
            }
            else if (Location.ToUpper().Trim() == "NEW YORK")
            {
                ConvertedPrice = price;
                Currency = "$";
            }
            else if (Location.ToUpper().Trim() == "STOCKHOLM")
            {
                ConvertedPrice = price * toSEK;
                Currency = "SEK";
            }
            else
            {

            }
        }

        public int Id { get;  set; }
        public string Location { get; set; }
        public string Currency { get; set; }
        public double ConvertedPrice { get; set; }
    }
}
