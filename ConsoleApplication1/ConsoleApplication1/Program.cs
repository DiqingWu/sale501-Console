using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static private int _consoleState = 0;
        static private bool _nextState = false;
        
        static void Main(string[] args)
        {

            Console.WriteLine("Hello to the sale system! welcome!");
            MainMenu();
            Console.WriteLine("Thanks you for using our system, type anything to exit!");
            Console.ReadLine();
        }
        /// <summary>
        /// MainMenu
        /// </summary>
        static private void MainMenu()
        {

            while (!_nextState) 
            // passing ball system, when any sub-manu use ture, but on main use false
            // When pass back ture, it will initially exit program.
            {
                try
                {
                    Console.WriteLine("Main Menu:");
                    Console.WriteLine("Type an int tell us what would you want to do:");
                    Console.WriteLine("(1)CreateSaleTransactionMenu:");
                    Console.WriteLine("(2)ReturnItemsMenu:");
                    Console.WriteLine("(3)EnterRebateMenu:");
                    Console.WriteLine("(4)GenerateRebateChek:");
                    Console.WriteLine("(5)Exit System:");
                    Console.Write("Please type integer 1-5: ");
                    string input = Console.ReadLine();

                    _consoleState = Convert.ToInt32(input);
                    switch (_consoleState)
                    {
                        case 1:
                            _nextState = true;
                            CreateSaleTransactionMenu();
                            break;
                        case 2:
                            _nextState = true;
                            ReturnItemsMenu();
                            break;
                        case 3:
                            _nextState = true;
                            EnterRebateMenu();
                            break;
                        case 4:
                            _nextState = true;
                            GenerateRebateCheck();
                            break;
                        case 5:
                            _nextState = true;
                            break;
                        default:
                            Console.WriteLine("Wrong type of input, please type single integer 1-5.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error due to: ");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("\nDue to error input, Starting over... ... \n");
                }
            }
        }
        /// <summary>
        /// CreateSaleTransactionMenu display this menu when mainmenu called
        /// also have function to add data to database.
        /// </summary>
        static private void CreateSaleTransactionMenu()
        {
            while (_nextState)
            {
                try
                {
                    Console.WriteLine("\nCreate Sale Transaction Menu:");
                    Console.WriteLine("Please fill out the following information by Type Words to create a sale transaction: ");

                    Console.Write("First Name: ");
                    string firstName = Console.ReadLine();

                    Console.Write("Last Name: ");
                    string lastName = Console.ReadLine();

                    Console.Write("Address: ");
                    string address = Console.ReadLine();

                    Console.Write("Email: ");
                    string email = Console.ReadLine();

                    int count = 0;
                    Queue<string> storge = new Queue<string>();
                    string input;
                    do
                    {
                        Console.Write("Enter Name of your " + (count+1) +" item that you bought: ");
                        input = Console.ReadLine();
                        storge.Enqueue(input);
                        Console.Write("Enter Cost of your " + (count+1) + " item: $");;
                        input = Console.ReadLine();
                        storge.Enqueue(input);
                        do
                        {
                            Console.Write("Do you want Enter another item? (y/n): ");
                            input = Console.ReadLine().ToLower();
                        } while (input != "n" &&input != "y");
                        count++; 
                    } while (input != "n");

                    Console.WriteLine("\nConfirm with Your information: ");

                    Console.WriteLine("First Name: " + firstName);
                    
                    Console.WriteLine("Last Name: " + lastName);

                    Console.WriteLine("Address: " + address);

                    Console.WriteLine("Email: " + email);

                    string[] items = new string[count];
                    int[] costs = new int[count];
                    //count = 0;
                    for(int i =0;i<count;i++)
                    {
                        items[i] = storge.Dequeue();
                        costs[i] =Convert.ToInt32( storge.Dequeue());
                        Console.WriteLine("Number " +(i+1) +" item, "+items[i]+", cost $"+costs[i]);

                    }

                    do
                    {
                        Console.Write("Would you confirm this Transaction?(y/n)(Warning: selec n will start over or exit current stage)?: ");
                        input = Console.ReadLine().ToLower();
                    } while (input != "n" && input != "y");

                    if(input == "y")
                    {
                        
                        int id =  DataBase.idGenerator();
                        DataBase.AddTransaction(firstName,lastName,items,address,costs,email,id,DataBase._currentDate);
                        Console.WriteLine("\nCurrent Date is been set to 2018 June 1st.");
                        Console.WriteLine("Your Transition ID is:" + DataBase.getIdFormat(id));
                        Console.WriteLine("Transaction been successfuly add in to DataBase.\n");
                    }
                    else
                    {
                        Console.WriteLine("Transaction Failed add in to DataBase.");
                    }


                    do
                    {
                        Console.Write("Would you like to add another Transaction? (y/n): ");
                        input = Console.ReadLine().ToLower();
                    } while (input != "n" && input != "y");

                    if(input == "n")
                    {
                        Console.WriteLine("Backing to MainMenu...");
                        _nextState = false;
                    }
                    Console.WriteLine();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error due to: ");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("\nDue to error input, Starting over... ... \n");
                }
            }

        }

        static private void ReturnItemsMenu()
        {
            while (_nextState)
            {
                try
                {
                    string input = "";
                    Console.WriteLine("\nReturn Items Menu:");
                    Console.Write("Please type your Transaction ID (example:14): ");
                    int id = Convert.ToInt32( Console.ReadLine());
                    if(!DataBase.CheckTransactionExist(id))
                        Console.WriteLine("\nID given was not exist!\n");
                    else
                    {
                        
                        Console.WriteLine("\nId given is found. Here are List of items and cost.");
                        Console.WriteLine(DataBase.GetItemCostList(id));
                        bool loop = true;
                        while (loop)
                        {
                            Console.Write("Which Item would you like to return?(type item name): ");
                            string item= Console.ReadLine();
                            
                            Console.WriteLine( DataBase.ReturnItem(id,item,DataBase._currentDate));

                            do
                            {
                                Console.Write("Would you like to return an another item? (y/n): ");
                                input = Console.ReadLine().ToLower();
                            } while (input != "n" && input != "y");

                            if (input == "n")
                            {
                                loop = false;
                            }
                        }
                    }


                    do
                    {
                        Console.Write("Would you like to return an another Transaction? (y/n): ");
                        input = Console.ReadLine().ToLower();
                    } while (input != "n" && input != "y");

                    if (input == "n")
                    {
                        Console.WriteLine("Backing to MainMenu...");
                        _nextState = false;
                    }
                    Console.WriteLine();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error due to: ");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("\nDue to error input, Starting over... ... \n");
                }
            }
        }
        static private void EnterRebateMenu()
        {
            while (_nextState)
            {
                try
                {
                    string input = "";
                    Console.WriteLine("Enter Rebate Menu:");
                    Console.Write("Enter ID:");
                    int id = Convert.ToInt32( Console.ReadLine());
                    Console.Write("Enter amount of rebate.(example,if 11% enter 11):");
                    double rebate = Convert.ToDouble(Console.ReadLine());
                    Console.Write("Enter expire date for rebate.(MM/DD/YYYY): ");
                    string[] line =  Console.ReadLine().Split('/');
                    Console.WriteLine( DataBase.EnterRebate(id, rebate, new DateTime(Convert.ToInt32(line[2]), Convert.ToInt32(line[0]), Convert.ToInt32(line[1]))));




                    do
                    {
                        Console.Write("Would you like to add another rebate? (y/n): ");
                        input = Console.ReadLine().ToLower();
                    } while (input != "n" && input != "y");

                    if (input == "n")
                    {
                        Console.WriteLine("Backing to MainMenu...");
                        _nextState = false;
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error due to: ");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("\nDue to error input, Starting over... ... \n");
                }


            }
        }
        static private void GenerateRebateCheck()
        {
            while (_nextState)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine( DataBase.GenerateRebateCheck());
                    _nextState = false;
                    Console.WriteLine("Output printed, type anything to continue(will be back to main manu)!");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
