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
            MainMenu();
        }

        static void MainMenu()
        {

            while (true)
            {
                
                Console.WriteLine("-----------------------------");
                Console.WriteLine("1) Create asset");
                Console.WriteLine("2) Update asset");
                Console.WriteLine("3) Delete asset");
                Console.WriteLine("4) List all assets");
                Console.WriteLine("-----------------------------");

                ConsoleKeyInfo inputKey = Console.ReadKey();

                if(inputKey.Key == ConsoleKey.D1)
                {
                    CreateAsset();
                }
                if (inputKey.Key == ConsoleKey.D2)
                {
                    UpdateAsset();
                }
                if (inputKey.Key == ConsoleKey.D3)
                {
                    DeleteAsset();
                }
                if (inputKey.Key == ConsoleKey.D4)
                {
                    ListAssets();
                }
                Console.WriteLine("\n");
                //Console.ReadLine();

            }
        }

        static void SetupData()
        {
            Console.WriteLine("Clearing database");
            Console.WriteLine("Creating office objects");
            Console.WriteLine("Adding sample assets to database");
            Console.WriteLine("-----------------------");


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

        static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            MainMenu();
        }

        static void CreateAsset()
        {
            Console.WriteLine("\n");
            Console.WriteLine("C to add computer, M to add mobile");
            string typeToAdd = Console.ReadLine();

            if (typeToAdd.ToUpper().Trim() == "C" || typeToAdd.ToUpper().Trim() == "M")
            {
            }
            else
            {
                ErrorMessage("ERROR! C or M is the options for what to create!");

            }

            Console.WriteLine("Model name: ");
            string modelName = Console.ReadLine();

            int nameConflicts = db.Assets.Where(asset => asset.ModelName.Contains(modelName)).Count();
           
            if (nameConflicts > 0)
            {
                ErrorMessage("CONFLICT ERROR! Model name already taken! \n");  
            }

            Console.WriteLine("Enter an office (London, New York or Stockholm)): ");
            string location = Console.ReadLine();
            string locationFormatted = location.ToUpper().Trim();
            var officeToAdd = new Office();

            int correctOffices = db.Offices.Where(o => o.Location.Contains(locationFormatted)).Count();

            if (correctOffices > 0)
            {
                officeToAdd = db.Offices.Where(o => o.Location.Contains(locationFormatted)).FirstOrDefault();
            }
            else
            {
                ErrorMessage("ERROR! Please input either London, New York or Stockholm as office!");   
            }

            Console.WriteLine("Price (US $): ");
            string priceStr = Console.ReadLine();
            bool priceAsDouble = double.TryParse(priceStr, out double price);

            if (!priceAsDouble)
            {

                ErrorMessage("ERROR! Price must be a number!");
                
            }


            Console.WriteLine("Purchase date: (Written like XXXX-XX-XX)");
            string purchaseDateStr = Console.ReadLine();
            bool purchaseAsDate = DateTime.TryParse(purchaseDateStr, out DateTime purchaseDate);

            if (!purchaseAsDate)
            {

                ErrorMessage("ERROR! Date must be written as a proper date");
                
            }

            if (purchaseDate < new DateTime(1950, 01, 01))
            {

                ErrorMessage("ERROR! Date must be written as a proper date");
                
            }

            if (typeToAdd.ToUpper().Trim() == "C")
            {
                double convertedPrice = price * officeToAdd.ExchangeRate;
                Computer com = new Computer(modelName, convertedPrice, officeToAdd, purchaseDate);
                db.Assets.Add(com);
                db.SaveChanges();
            }
            else if (typeToAdd.ToUpper().Trim() == "M")
            {
                double convertedPrice = price * officeToAdd.ExchangeRate;
                Mobile mob = new Mobile(modelName, convertedPrice, officeToAdd, purchaseDate);
                db.Assets.Add(mob);
                db.SaveChanges();
            }
        }

        static void DeleteAsset()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Which asset to delete? Enter ID:");
            string id = Console.ReadLine();
            bool idAsInt = int.TryParse(id, out int idInt);

            if (idAsInt)
            {
                Asset asset = db.Assets.Find(idInt);
                db.Assets.Remove(asset);
                db.SaveChanges();

                Console.WriteLine("Asset with ID " + idInt + " deleted!");
            }
            else
            {
                ErrorMessage("Please write a valid number as ID!");
            }
        }

        static void UpdateAsset()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Update which asset? Enter ID:");
            string id = Console.ReadLine();
            bool idAsInt = int.TryParse(id, out int idInt);

            if(idAsInt)
            {
                Console.WriteLine("Give the asset a new name:");
                string newName = Console.ReadLine();

                Asset asset = db.Assets.Find(idInt);
                asset.ModelName = newName;
                db.Assets.Update(asset);
                db.SaveChanges();

                Console.WriteLine("Asset updated! New name is: " + newName);
            }
            else
            {
                ErrorMessage("Please write a valid number as ID!");
            }

        }

        static void ListAssets()
        {
            Console.WriteLine("\n");
            var sortedList = db.Assets.OrderBy(asset => asset.Office.Location).ThenBy(asset => asset.PurchaseDate).ToList();
            //List<Asset> sortedList = assetList.OrderBy(asset => asset.Office.Location).ThenBy(asset => asset.PurchaseDate).ToList();

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("ID:".PadRight(10) + "Type:".PadRight(15) + "Model:".PadRight(20) + "Office:".PadRight(15) + "Office ID:".PadRight(15) + "Local Price:".PadRight(15) + "Currency:".PadRight(15) + "Purchase Date:".PadRight(15));
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
                Console.WriteLine(a.Id.ToString().PadRight(10) + a.GetType().Name.ToString().PadRight(15) + a.ModelName.PadRight(20)
                    + a.Office.Location.PadRight(15) + a.Office.Id.ToString().PadRight(15) + a.Office.ExchangeRate.ToString().PadRight(15)
                    + a.Office.CurrencySymbol.PadRight(15) + a.PurchaseDate.ToShortDateString().PadRight(15)); ;
            }
            Console.ResetColor();
            Console.WriteLine("-----------------------------------------------------------------------------------");
        }
    }
}


