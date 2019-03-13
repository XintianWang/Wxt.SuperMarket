using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wxt.OnlineSuperMarket.Business.Services;

namespace Wxt.OnlineSuperMarket.Test.DebugConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SuperMarketService superMarketService = new SuperMarketService();
            while (true)
            {
                Console.WriteLine("Input the command:");
                string command = Console.ReadLine();
                if (command.StartsWith("addproduct"))
                {
                    string[] commands = command.Split(' ');
                    Console.WriteLine(superMarketService.AddProuct(commands[1], decimal.Parse(commands[2])));
                }
            }
        }
    }
}
