namespace Wxt.OnlineSuperMarket.UI.ConsoleApp
{
    using System;
    using Wxt.OnlineSuperMarket.Business.Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            CustomerService customerService = new CustomerService();
            SuperMarketService marketService = new SuperMarketService();
            bool canExist = false;
            while (!canExist)
            {
                Console.WriteLine("===============================================");
                Console.WriteLine("Command List:");
                Console.WriteLine("addcustomer name password, login username password, logout, deleteCustomer");
                Console.WriteLine("listproducts, liststocks, pickup productid count, putback productid count, clearcart, listcart, checkout");
                Console.WriteLine("addproduct name price, removeproduct id, increasestock id count, decreasestock id count, listreceipts");
                Console.WriteLine("Please input command:");
                string command = Console.ReadLine().Trim();
                string[] commands = command.Split(' ');
                try
                {
                    switch (commands[0].ToLowerInvariant())
                    {
                        case "addcustomer":
                            if (commands.Length < 3)
                            {
                                Console.WriteLine("Needs two parameters.");
                            }
                            var customer = customerService.AddCustomer(commands[1], commands[2]);
                            Console.WriteLine("Add new customer succeeded.");
                            Console.WriteLine(customer.ToString());
                            break;

                        case "login":
                            if (commands.Length < 3)
                            {
                                Console.WriteLine("Needs two parameters.");
                            }
                            customerService.Login(commands[1], commands[2]);
                            Console.WriteLine("Customer login.");
                            Console.WriteLine("Welcome, you can choose anything you want.");
                            break;

                        case "logout":
                            if (customerService.Logout())
                            {
                                Console.WriteLine("Customer logout.");
                            }
                            break;

                        case "deletecustomer":
                            if (customerService.DeleteCustomer())
                            {
                                Console.WriteLine("Customer deleted.");
                            }
                            break;

                        case "listproducts":
                            Console.WriteLine(marketService.ListAllProducts());
                            break;

                        case "liststocks":
                            Console.WriteLine(marketService.ListAllStocks());
                            break;

                        case "listreceipts":
                            Console.WriteLine(marketService.ListAllReceipts());
                            break;

                        case "pickup":
                            if (commands.Length < 3)
                            {
                                Console.WriteLine("Needs two parameters.");
                            }
                            customerService.AddToCart(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine($"Pick up products succeeded.");
                            break;

                        case "putback":
                            if (commands.Length < 3)
                            {
                                Console.WriteLine("Needs two parameters.");
                            }
                            var realCount = customerService.RemoveFromCart(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine($"Put back {realCount} products succeeded.");
                            break;

                        case "listcart":
                            Console.WriteLine(customerService.ListCart());
                            break;

                        case "clearcart":
                            customerService.ClearCart();
                            Console.WriteLine("Clear shopping cart succeeded.");
                            break;

                        case "checkout":
                            Console.WriteLine(customerService.CheckOut());
                            break;
                        case "addproduct":
                            if (commands.Length < 3)
                            {
                                Console.WriteLine("Needs two parameters.");
                            }
                            var product = marketService.AddProuct(commands[1], decimal.Parse(commands[2]));
                            Console.WriteLine("Add new product succeeded.");
                            Console.WriteLine(product.ToString());
                            break;

                        case "removeproduct":
                            if (commands.Length < 2)
                            {
                                Console.WriteLine("Needs one parameter.");
                            }
                            marketService.RemoveProduct(int.Parse(commands[1]));
                            Console.WriteLine("Product deleted.");
                            break;

                        case "increasestock":
                            if (commands.Length < 3)
                            {
                                Console.WriteLine("Needs two parameters.");
                            }
                            marketService.IncreaseStockt(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine($"Increase stock succeeded.");
                            break;

                        case "decreasestock":
                            if (commands.Length < 3)
                            {
                                Console.WriteLine("Needs two parameters.");
                            }
                            marketService.DecreaseStockt(int.Parse(commands[1]), int.Parse(commands[2]));
                            Console.WriteLine($"Decrease stock succeeded.");
                            break;

                        case "q":
                            canExist = true;
                            break;

                        default:
                            Console.WriteLine("Invalid command.");
                            break;
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
