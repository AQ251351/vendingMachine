using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using static vendingMachine.Program;

namespace vendingMachine
{
    internal class Program
    {
        public const string FILENAME = "ProductFile.txt";
        public const int QUIT_FUNCTION = 99;
        public const int ADMIN_FUNCTION = 999999;

        public struct Product
        {
            public string name;
            public double price;
            public int productCount;
        };

        static void Main(string[] args)
        {
            
            List<Product> machine = new List<Product>();
            // load our product data hear.
            LoadItemsFromFile(machine);

            while (true)
            {
                //Display the welcome screen and getting input from user
                int optionChose = WelcomeScreen(machine);
                //99 is the option to exit the program
                //this means right now our vending machine can hold 98 products
                if (optionChose == QUIT_FUNCTION)
                {
                    WriteProductsBack(machine);
                    Environment.Exit(0);
                }
                else if (optionChose == ADMIN_FUNCTION)
                {
                    AdminFeatures(machine);
                }
                else
                {
                    EnterPayment(machine, optionChose);
                }
            }
        }

        /********************************************
         *  method : AdminFeatures
         *  params : List of products
         *  return : 
         *
         * Description 
         *   This method will need to be able to add and remove stock to our stock list.
         ********************************************/
        private static void AdminFeatures(List<Product> machine)
        {
            Console.WriteLine("===========================");
            Console.WriteLine("    UNDER CONSTRUCTION     ");
            Console.WriteLine("===========================");
            Thread.Sleep(3000);
            return;
        }

        /**********************************************
        *  method : WriteProductsBack
        *  params : List of products
        *  return : boolean  indicates success or faliure 
        *  
        *  Description 
        *   This will write our products back to the file
        * *******************************************/
        private static bool WriteProductsBack(List<Product> machine)
        {
            const string DELIMITER = ",";
            StreamWriter filewriter = null;
            bool rc = true;
            try
            {
                filewriter = new StreamWriter(FILENAME);
                foreach (Product product in machine)
                {
                    String productLine = product.name + DELIMITER + product.price + DELIMITER + product.productCount;
                    filewriter.WriteLine(productLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem has accured when writing our products into the file");
                Console.WriteLine("Technical Details : " + ex.Message);
                rc = false;
            }
            if (filewriter != null) 
            {
                filewriter.Close(); 
            }
            return rc;
        }

        /**********************************************
         *  method : LoadItemsFromFile
         *  params : List of products
         *  return : void
         *  
         *  Description 
        *   This will load our products from a file
        * *******************************************/

        private static void LoadItemsFromFile(List<Product> machine)
        {
           
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(FILENAME))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {

                            if (line.StartsWith("#"))
                            {
                                // this line is a comment. ignore it
                                continue;
                            }
                            else
                            {
                                // we are assuming this is a product line
                                string [] details = line.Split(',');
                                if (details.Length != 3)
                                {
                                    Console.WriteLine("This entry in the Product File is not in the valid format : " + line);
                                }
                                else
                                {
                                    // looks good with our product details. lets construct it
                                    Product product = new Product();
                                    product.name = details[0];
                                    try
                                    {
                                        product.price = double.Parse(details[1]);
                                        product.productCount = int.Parse(details[2]);


                                        machine.Add(product);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("This entry in the Product File is not in the valid format : " + line);
                                        Console.WriteLine("Technical Details : " + ex.Message);
                                    }
                                }
                            }
                        } 

                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("There is a problem opening the product file " + FILENAME);
                Console.WriteLine("We cannot continue ");
                Console.WriteLine("Technical Details : " + e.Message);
                Environment.Exit(1);
            }




        }

        /**********************************************
         *  method : EnterPayment
         *  params : List of products
         *           the option the user chose
         *  return : void
         *  
         *  Description 
         *   This method takes the payment from the user 
         * and gives them change
        * *******************************************/
        
        private static void EnterPayment(List<Product> machine, int optionChose)
        {
            Product chosenProduct = machine[optionChose];
            double price = chosenProduct.price;

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

                    Console.WriteLine("Here is your product " + chosenProduct.name);
                    //decrement our product count
                    chosenProduct.productCount--;
                    machine[optionChose] = chosenProduct;
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
                    

                    if (amountOwe < 1)
                    {
                        amountOwe *= 100;
                        Console.WriteLine("You still owe " + amountOwe + " pence");
                    }
                    else
                    {
                        
                        Console.WriteLine("You still owe £" + amountOwe );
                    }
                }

            } // end while




        }

        /**********************************************
      *  method : WelcomeScreen
      *  params : List of products
      *  return : The index of the product chosen or 99 to exit
      *  Description 
      *   Welcome screen displays the list of products 
      *   asks the user which product they want
     * *******************************************/

        public static int WelcomeScreen(List<Product>machine)
        {
            int userPurchase = 0;
            bool valid = false;
            while (valid == false)
            {


                int count = 0;
                //Console.Clear();
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
                Console.WriteLine("Enter ADMIN to enter the stock administrator function");
                Console.WriteLine("Enter " + QUIT_FUNCTION +" to exit ");
                

                Console.WriteLine("\nPlease enter the number of the product you wish to buy.");

                String userInput = Console.ReadLine();

                if (userInput == null && userInput.Length == 0)
                {
                    continue;
                }
                if ("ADMIN".Equals (userInput.ToUpper()))
                {
                    return ADMIN_FUNCTION;
                }
                
                try
                {
                    userPurchase = int.Parse(userInput);

                    if (userPurchase >= 0 && userPurchase < machine.Count)
                    {
                        if (machine[userPurchase].productCount == 0)
                        {
                            Console.WriteLine("You cannot select " + machine[userPurchase].name +" it is out of stock");
                            Thread.Sleep(3000);
                            continue;
                        }
                        valid = true;
                    }
                    else if (userPurchase == QUIT_FUNCTION)
                    {
                        Console.WriteLine("You have chosen to exit ,thank you.");
                        valid = true;
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
         * 
         * NOTE : deprecated. Currently unused
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
