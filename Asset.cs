using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject_02_EF_AssetTracking
{
    class Asset
    {
    

        public Asset()
        {
            
        }

        public Asset(string modelName, double price, Office office, DateTime purchaseDate)
        {
            PurchaseDate = purchaseDate;
            ExpirationDate = purchaseDate.AddYears(3);
            ModelName = modelName;
            Price = price;

            Office = office;
            //Office = office;

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("Expiration date is: " + ExpirationDate.ToShortDateString());
        }

        //public void GetRegion(double price)
        //{
        //    

        //    if (Location.ToUpper().Trim() == "LONDON")
        //    {
        //        ConvertedPrice = price * toUK;
        //        Currency = "£";
        //    }
        //    else if (Location.ToUpper().Trim() == "NEW YORK")
        //    {
        //        ConvertedPrice = price;
        //        Currency = "$";
        //    }
        //    else if (Location.ToUpper().Trim() == "STOCKHOLM")
        //    {
        //        ConvertedPrice = price * toSEK;
        //        Currency = "SEK";
        //    }
        //    else
        //    {

        //    }
        //}
double toSEK = 8.67;
        //    double toUK = 0.72;
        public int Id { get; set; }
        public string ModelName { get; set; }
        public double Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpirationDate { get; }
        public Office Office { get; set; }

    }
}
