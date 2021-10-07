using System;

namespace MiniProject_02_EF_AssetTracking
{
    class Mobile : Asset
    {
        public Mobile(string modelName, double price, Office office, DateTime purchaseDate)
                   : base(modelName, price, office, purchaseDate)
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Mobile " + " " + modelName + " " + " successfully added! \n");
            Console.ResetColor();
            Console.WriteLine("-----------------------------------------------------------");
        }
    }
}