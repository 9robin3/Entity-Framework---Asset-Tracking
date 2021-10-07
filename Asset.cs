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

        public int Id { get; set; }
        public string ModelName { get; set; }
        public double Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpirationDate { get; }
        public Office Office { get; set; }

    }
}
