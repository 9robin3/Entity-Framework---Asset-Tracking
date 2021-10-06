using System;

namespace MiniProject_02_EF_AssetTracking
{
    class Computer : Asset
    {
        public Computer(string modelName, double price, string officeLocation, DateTime purchaseDate)
                    : base(modelName, price, officeLocation, purchaseDate)
        {

            Console.WriteLine("-----------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Computer " + " " + modelName + " " + " successfully added! \n");
            Console.ResetColor();
            Console.WriteLine("-----------------------------------------------------------");
        }
    }
}