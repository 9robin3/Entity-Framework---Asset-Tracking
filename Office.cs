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

        public Office(string location, double exRate, string currSymbol)
        {
            Location = location;
            ExchangeRate = exRate;
            CurrencySymbol = currSymbol;

            //GetRegion(price);
        }

        public int Id { get;  set; }
        public string Location { get; set; }
        public string CurrencySymbol { get; set; }
        public double ExchangeRate { get; set; }
    }
}
