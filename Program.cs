using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MiniProject_02_EF_AssetTracking
{
    class Program
    {
        static Context db = new Context();

        static void Main(string[] args)
        {

            ////Where do I create the Office objects??

            ////How and where do I connect a specific Office (Location) with an asset?
            
            //Create and add the offices to the db
            Office offNewYork = new Office("NEW YORK".ToUpper().Trim());
            Office offStockholm = new Office("STOCKHOLM".ToUpper().Trim());
            Office offLondon = new Office("LONDON".ToUpper().Trim());

            db.Offices.AddRange(offNewYork, offStockholm, offLondon);
            db.SaveChanges();

            //TEST CODE - ADD SAMPLE ASSETS
            Mobile mob = new Mobile("Samsung Galaxy S20",8999, offNewYork, new DateTime(2021-01-01));
            Mobile mob2 = new Mobile("Nokia X450", 5799, offNewYork, new DateTime(2021 - 01 - 01));
            Computer cpu = new Computer("CPU1", 13500, offLondon, new DateTime(2001 - 01 - 06));
            db.Add(mob);
            db.Add(mob2);
            db.SaveChanges();


            //TEST CODE - CLEAR DB
            //db.RemoveRange(db.Assets);
            //db.SaveChanges();

            

            //Asset offQuery = db.Assets.Where(c => c.Office.Location.Contains(offNewYork.Location)).FirstOrDefault();

            //List<Asset> assetList = new List<Asset>();
            //////Context DB instead of list when adding?????
            ///

            //while (true)
            //{
            //    Console.ResetColor();
            //    Console.WriteLine("Type C (computer) or M (mobile) to create assets, Q to print and quit");
            //    string input = Console.ReadLine();
            //    if (input.ToUpper().Trim() == "Q")
            //    {
            //        ListAssets(assetList);
            //        //break;
            //    }
            //    else if (input.ToUpper().Trim() == "C" || input.ToUpper().Trim() == "M")
            //    {
            //        AddAsset(input, assetList);
            //    }

            Console.ReadLine();

            }

        static void AddAsset(string type)
        {
            Console.WriteLine("Model name: ");
            string modelName = Console.ReadLine();

            int nameConflicts = db.Assets.Where(asset => asset.ModelName.Contains(modelName)).Count();
            if (nameConflicts > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CONFLICT ERROR! Model name already taken! \n");
                return;
            }

            Console.WriteLine("Enter an office (London, New York or Stockholm)): ");
            string location = Console.ReadLine();
            string locationFormatted = location.ToUpper().Trim();

            if (locationFormatted == "LONDON" || locationFormatted == "NEW YORK" || locationFormatted == "STOCKHOLM")
            {
                ///I want to set up the office here, depedning on input
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR! Please input either London, New York or Stockholm as office!");
                return;
            }

            Console.WriteLine("Price (US $): ");
            string priceStr = Console.ReadLine();
            bool priceAsDouble = double.TryParse(priceStr, out double price);

            if (!priceAsDouble)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR! Price must be a number!");
                return;
            }

            Console.WriteLine("Purchase date: (Written like XXXX-XX-XX)");
            string purchaseDateStr = Console.ReadLine();
            bool purchaseAsDate = DateTime.TryParse(purchaseDateStr, out DateTime purchaseDate);

            if (!purchaseAsDate)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR! Date must be written as a proper date");
                return;
            }

            if (purchaseDate < new DateTime(1950, 01, 01))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR! Date must be written as a proper date");
                return;
            }

            if (type.ToUpper().Trim() == "C")
            {
                Computer com = new Computer(modelName, price, officeHere, purchaseDate);
                db.Assets.Add(com);
                db.SaveChanges();
            }

            else if (type.ToUpper().Trim() == "M")
            {
                Mobile mob = new Mobile(modelName, price, officeHere, purchaseDate);
                db.Assets.Add(mob);
                db.SaveChanges();
            }
        }

        static void DeleteAsset(string id)
        {
            Asset asset = db.Assets.Find(id);
            db.Assets.Remove(asset);
        }

        static void UpdateAsset(string id, string newName)
        {
            Asset asset = db.Assets.Find(id);
            asset.ModelName = newName;
            db.Assets.Update(asset);
            db.SaveChanges();
        }

        static void ListAssets()
        {
            var sortedList = db.Assets.OrderBy(asset => asset.Office.Location).ThenBy(asset => asset.PurchaseDate).ToList();
            //List<Asset> sortedList = assetList.OrderBy(asset => asset.Office.Location).ThenBy(asset => asset.PurchaseDate).ToList();

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Type".PadRight(10) + "Model".PadRight(15) + "Office:".PadRight(15) + "Price:".PadRight(10) + "Currency:".PadRight(10) + "Purchase Date:".PadRight(15));
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");

            foreach (Asset a in sortedList)
            {
                if (DateTime.Now > a.ExpirationDate.AddMonths(-3))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (DateTime.Now > a.ExpirationDate.AddMonths(-6))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.WriteLine(a.GetType().Name.ToString().PadRight(10) + a.ModelName.PadRight(15)
                    + a.Office.Location.PadRight(15) + a.Office.ConvertedPrice.ToString().PadRight(10)
                    + a.Office.Currency.PadRight(10) + a.PurchaseDate.ToShortDateString().PadRight(15)); ;
            }
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }
    }
}
