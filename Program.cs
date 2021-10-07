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
            SetupData();

            while (true)
            {
                Console.ResetColor();
                Console.WriteLine("Type C (computer) or M (mobile) to create assets, Q to print and quit");
                string input = Console.ReadLine();

                if (input.ToUpper().Trim() == "Q")
                {
                    ListAssets();
                    //break;
                }
                else if (input.ToUpper().Trim() == "C" || input.ToUpper().Trim() == "M")
                {
                    AddAsset(input);
                }

                Console.ReadLine();

            }

            static void SetupData()
            {
                db.RemoveRange(db.Assets);
                db.RemoveRange(db.Offices);
                db.SaveChanges();

                double toSEK = 8.67;
                double toUK = 0.72;
                double toUS = 1;

                Office offNewYork = new Office("NEW YORK", toUS, "$");
                Office offStockholm = new Office("STOCKHOLM", toSEK, ":-");
                Office offLondon = new Office("LONDON", toUK, "£");

                db.Offices.AddRange(offNewYork, offStockholm, offLondon);
                db.SaveChanges();

                var london = db.Offices.Where(o => o.Location.Contains("LONDON")).FirstOrDefault();
                var newyork = db.Offices.Where(o => o.Location.Contains("NEW YORK")).FirstOrDefault();
                var stockholm = db.Offices.Where(o => o.Location.Contains("STOCKHOLM")).FirstOrDefault();

                //TEST CODE - ADD SAMPLE ASSETS
                Mobile mob = new Mobile("Samsung Galaxy S20", 8999, london, new DateTime(2021 - 01 - 01));
                Mobile mob2 = new Mobile("Nokia X450", 5799, london, new DateTime(2021 - 01 - 01));
                Computer cpu2 = new Computer("CPU2", 18000, newyork, DateTime.Now);
                db.Add(mob);
                db.Add(mob2);
                db.Add(cpu2);

                db.SaveChanges();
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
                var officeToAdd = new Office();

                //if (locationFormatted == || locationFormatted == "NEW YORK" || locationFormatted == "STOCKHOLM")
                //{  

                //}

                if (db.Offices.Where(o => o.Location.Contains(locationFormatted)).Count() > 0)
                {
                    officeToAdd = db.Offices.Where(o => o.Location.Contains(locationFormatted)).FirstOrDefault();
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
                    Computer com = new Computer(modelName, price * officeToAdd.ExchangeRate, officeToAdd, purchaseDate);
                    db.Assets.Add(com);
                    db.SaveChanges();
                }

                else if (type.ToUpper().Trim() == "M")
                {
                    Mobile mob = new Mobile(modelName, price * officeToAdd.ExchangeRate, officeToAdd, purchaseDate);
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
                        + a.Office.Location.PadRight(15) + a.Office.ExchangeRate.ToString().PadRight(10)
                        + a.Office.CurrencySymbol.PadRight(10) + a.PurchaseDate.ToShortDateString().PadRight(15)); ;
                }
                Console.WriteLine("-----------------------------------------------------------------------------------");
            }
        }
    }
}

