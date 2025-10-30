using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace vendingMachine
{
    internal class Program
    {
        public struct Product
        {
            public string name;
            public double price;
            public int productCount;
        };

        static void Main(string[] args)
        {
            Product[] machine = new Product[10];
            // load our product data hear.
            LoadItems(machine);

            while (true)
            {
                //Display the welcome screen and getting input from user
                int optionChose = WelcomeScreen(machine);

                EnterPayment(machine, optionChose);
            }
        }

        /****************************************************
         * This method takes the payment from the user 
         * and gives them change
         ****************************************************/
        private static void EnterPayment(Product[] machine, int optionChose)
        {
            // This assume the caller is passing a valid index!
            double price = machine[optionChose].price;

            bool quit = false;
            double moneyentered = 0;
            while (quit == false)
            {
                Console.WriteLine("Enter Coin: X = quit\n 1 = 5p, 2 = 10p, 3 = 20p, 4 = 50p, 5 = £1, 6 = £2");
                string coin = Console.ReadLine();
                switch (coin)
                {
                    case "1" :
                    {
                        moneyentered = moneyentered + 0.05;
                        break;
                    }
                    case "2":
                    { 
                        moneyentered = moneyentered + 0.1;
                        break;
                    }
                    case "3":
                    {
                        moneyentered = moneyentered + 0.2;
                        break;
                    }
                    case "4":
                    {
                        moneyentered = moneyentered + 0.5;
                        break;
                    }
                    case "5":
                    {
                        moneyentered = moneyentered + 1;
                        break;
                    }
                    case "6":
                    {
                        moneyentered = moneyentered + 2;
                        break;
                    }
                    case "X":
                    case "x":
                    {
                        //make sure they get there money back if they decide to quit
                        Console.WriteLine("You have decide to quit"); 
                        if (moneyentered >0 )
                        {

                            Console.WriteLine("You will be returned "+ moneyentered);

                        }
                        quit = true;
                        break;
                    }
                    default:
                    {
                        //if they enter a invalid coin go back to the loop
                        Console.Write("No such coin / invalid input");
                        continue;
                    }

                        
                } // end switch

                if (moneyentered >= price)
                {

                    Console.WriteLine("Here is your product " + machine[optionChose].name);
                    //decrement our product count
                    machine[optionChose].productCount--;
                    if (moneyentered > price)
                    {
                        moneyentered -= price;
                        Console.WriteLine("here is your change" + moneyentered);

                    }
                    Thread.Sleep(3000);
                    quit = true;

                }
                else 
                {
                    double amountOwe = price - moneyentered;
                    amountOwe *= 100;
                    Console.WriteLine("You still owe " + amountOwe + " pence");
                }

            } // end while




        }



        /****************************************************
         * 
         * 
         ****************************************************/
        public static int WelcomeScreen(Product[]machine)
        {
            int userPurchase = 0;
            bool valid = false;
            while (valid == false)
            {


                int count = 0;
                Console.Clear();
                Console.WriteLine("Here is are our list of products available ");
                Console.WriteLine("==========================================");
                foreach (Product item in machine)
                {
                    if (item.productCount == 0)
                    {
                        Console.WriteLine(item.name + " Out of stock");
                    }
                    else
                    {
                        Console.WriteLine("Item " + count + ".  " + item.name + ". Price £" + item.price + " Stock count " + item.productCount);
                    }
                    count++;
                        
                }
                Console.WriteLine("Enter 99 to exit ");

                Console.WriteLine("Please enter the number of the product you wish to buy.");
                
                try
                {
                    userPurchase = int.Parse(Console.ReadLine());

                    if (userPurchase >= 0 && userPurchase < machine.Length)
                    {
                        if (machine[userPurchase].productCount == 0)
                        {
                            Console.WriteLine("You cannot select " + machine[userPurchase].name +" it is out of stock");
                            Thread.Sleep(3000);
                            continue;
                        }
                        valid = true;
                    }
                    else if (userPurchase == 99)
                    {
                        Console.WriteLine("You have chosen to exit ,thank you.");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("You have not selected a valid option.");
                        Thread.Sleep(3000);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input.");
                    Thread.Sleep(3000);
                }

            }
            return userPurchase;
        }
        /****************************************************
         * This Loads the reference data of all the 
         * product that are stored
         * NOTE: at the moment the product data is hard coded
         * but we can chage is to load the data from a properties file 
         ****************************************************/
        public static void LoadItems(Product[] machine)
        {
            machine[0].name = "Twix";
            machine[0].price = 1.50;

            machine[1].name = "Mars bar";
            machine[1].price = 1.50;

            machine[2].name = "Wispa";
            machine[2].price = 1.00;

            machine[3].name = "Crisps";
            machine[3].price = 0.80;

            machine[4].name = "Energy drink";
            machine[4].price = 2.20;

            machine[5].name = "Water";
            machine[5].price = 1.00;

            machine[6].name = "Coke zero";
            machine[6].price = 1.50;

            machine[7].name = "Protein bar";
            machine[7].price = 2.20;

            machine[8].name = "Milky way";
            machine[8].price = 1.20;

            machine[9].name = "Malteser";
            machine[9].price = 1.40;


            // start with 10 of every product
            for(int i = 0; i < machine.Length; i++)
            {
                machine[i].productCount = 10;
            }
        }
    }
}
