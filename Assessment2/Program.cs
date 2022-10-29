using System;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Assessment2
{
    class Program
    {
        public static void Main(string[] args)
        {
            menu();
        }

        public static void menu()
        {
            Console.WriteLine("+------------------------------+\n| Welcome to the Auction House |\n+------------------------------+");
            Console.WriteLine(" ");
            Console.WriteLine("Main Menu\n-------- -\n(1) Register\n(2) Sign In\n(3) Exit");
            Console.WriteLine(" ");
            bool run = true;
            do
            {
                Console.WriteLine("Please select an option between 1 and 3: ");
                bool option = Int32.TryParse(Console.ReadLine(), out int optionValue);
                if (option)
                {
                    if (optionValue < 3 && optionValue > 0)
                    {
                        if (optionValue == 1)
                        {
                            Console.WriteLine(" ");
                            registration();

                        }
                        if (optionValue == 2)
                        {
                            Console.WriteLine(" ");
                            login();

                        }
                    }
                    if (optionValue > 2 || optionValue < 1)
                    {
                        Console.WriteLine("+--------------------------------------------------+\n| Good bye, thank you for using the Auction House! |\n+--------------------------------------------------+");
                        run = false;
                    }

                }
            } while (run);
            Environment.Exit(0);
        }

        //LOGGED IN USER FUNCTIONALITY START
        public static void login()
        {
            Data db = new Data();
            db.Path = "Users.txt";
            Console.WriteLine("Login\n------------");
            Console.WriteLine(" ");
            Console.WriteLine("Please enter your name");
            string username = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine("Enter your password");
            string password = Console.ReadLine();
            User user = new User(username, email, password);
            if(db.readFile(user))
            {
                //CHECK IF USER HAS PROVIDED ADDRESS BEFORE
                if (db.checkClientAddressFile(email))
                {
                    clientMenu(user);
                }
                else
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Personal Details for " + username + "(" + email + ")\r\n----------------------------------------------------\r\n");
                    Console.WriteLine(" ");
                    Console.WriteLine("Please provide your home address.");
                    Console.WriteLine(" ");
                    Console.WriteLine("Unit number (0 = none):");
                    bool unit = Int32.TryParse(Console.ReadLine(), out int unitNumber);
                    Console.WriteLine(" ");
                    Console.WriteLine("Street number:");
                    bool streetNum = Int32.TryParse(Console.ReadLine(), out int streetNumber);
                    Console.WriteLine(" ");
                    Console.WriteLine("Street Name:");
                    string streetName = Console.ReadLine();
                    Console.WriteLine(" ");
                    Console.WriteLine("Street suffix:");
                    string streetSuffix = Console.ReadLine();
                    Console.WriteLine(" ");
                    Console.WriteLine("City:");
                    string city = Console.ReadLine();
                    Console.WriteLine(" ");
                    Console.WriteLine("State (ACT, NSW, NT, QLD, SA, TAS, VIC, WA):");
                    string state = Console.ReadLine();
                    Console.WriteLine(" ");
                    Console.WriteLine("Postcode (1000 .. 9999):");
                    bool post = Int32.TryParse(Console.ReadLine(), out int postcode);
                    Console.WriteLine(" ");
                    Address address = new Address(unitNumber, streetNumber, streetName, streetSuffix, city, postcode, state);
                    if (address.checkUnit(address.unit) && address.checkStreetNumber(address.streetNumber) && address.checkStreetName(address.streetName) && address.checkCity(address.city) &&  address.checkPostcode(address.postcode) && address.checkState(address.state))
                    {
                        if(db.addAddress(email, address))
                        {
                            clientMenu(user);
                        }
                    }
                }         
            }
            else
            {
                Console.WriteLine("Incorrect Email/Password");
            }
        }
        public static void clientMenu(User user)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Client Menu\r\n-----------\r\n(1) Advertise Product\r\n(2) View My Product List\r\n(3) Search For Advertised Products\r\n(4) View Bids On My Products\r\n(5) View My Purchased Items\r\n(6) Log off ");
            Console.WriteLine(" ");
            Console.WriteLine("Choose from 1-6");
            bool option = Int32.TryParse(Console.ReadLine(), out int optionValue);
            if (option)
            {
                //PRODUCT ADVERSTISEMENT START
                if(optionValue == 1)
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Product Advertisement for " + user.name + "(" + user.email + ")\r\n---------------------------------------------------------------------------------");
                    Console.WriteLine(" ");
                    Console.WriteLine("Product Name");
                    string productName = Console.ReadLine();
                    Console.WriteLine(" ");
                    Console.WriteLine("Product Description");
                    string productDescription = Console.ReadLine();
                    Console.WriteLine(" ");
                    Console.WriteLine("Product price ($d.cc)");
                    string productPrice = Console.ReadLine();
                    Console.WriteLine(" ");
                    Product product = new Product(productName, productDescription, productPrice);
                    Data db = new Data();
                    if (db.checkClientProductFile(user.email))
                    {
                        if(db.addProducts(user.email, product))
                        {
                            db.addAllProducts(product, user.email);
                            Console.WriteLine("Successfully added product " + product.productName + ", " + product.productDescription + ", " + product.price + ".");
                            Console.WriteLine(" ");
                            clientMenu(user);
                        }
                    } else
                    {
                        if (db.clientProductFile(user.email))
                        {
                            if (db.addProducts(user.email, product))
                            {
                                db.addAllProducts(product, user.email);
                                Console.WriteLine("Successfully added product " + product.productName + ", " + product.productDescription + ", " + product.price + ".");
                                Console.WriteLine(" ");
                                clientMenu(user);
                            }
                        }
                    }
                }
                //PRODUCT ADVERSTISEMENT END

                //PRODUCT LIST START
                if (optionValue == 2)
                {
                    Console.WriteLine("Product List for " + user.name + "(" + user.email + ")\r\n------------------------------------------------");
                    Console.WriteLine(" ");
                    Console.WriteLine("Item #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                    Data db = new Data();
                    db.readUserProductFile(user.email);
                    Console.WriteLine(" ");
                    clientMenu(user);
                }
                //PRODUCT LIST END

                //PRODUCT SEARTCH START
                if (optionValue == 3)
                {
                    Console.WriteLine("Product Search for " + user.name + "(" + user.email + ")\r\n---------------------------------------------------------------------------------");
                    Console.WriteLine(" ");
                    Console.WriteLine("Please supply a search phrase (ALL to see all products)");
                    string searchInput = Console.ReadLine().ToUpper();
                    if(searchInput == "ALL")
                    {
                        Data db = new Data();
                        if(db.checkAllProductsFile())
                        {
                            Console.WriteLine("Item #\tProduct name\tDescription\tList price\tBidder name\tBidder email\tBid amt");
                            db.readAllProductsFile(null, null, null);
                            Console.WriteLine(" ");
                            Console.WriteLine("Would you like to place a bid on any of these items (yes or no)?");
                            string answer = Console.ReadLine();
                            if (answer.ToUpper() == "YES")
                            {
                                Console.WriteLine(" ");
                                Console.WriteLine("Please select a product using their item number");
                                bool selected = Int32.TryParse(Console.ReadLine(), out int selectedValue);
                                db.selectedProduct(selectedValue, null, user);
                                Console.WriteLine(" ");
                                Console.WriteLine("How much do you bid?");
                                string userBid = Console.ReadLine();
                                db.selectedProduct(selectedValue, userBid, user);
                                Console.WriteLine(" ");
                                Console.WriteLine("Delivery Instructions\r\n---------------------\r\n(1) Click and collect\r\n(2) Home Delivery");
                                string selection = Console.ReadLine();
                                if (selection == "1")
                                {

                                }
                                else if (selection == "2")
                                {
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Please provide your home address.");
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Unit number (0 = none):");
                                    bool unit = Int32.TryParse(Console.ReadLine(), out int unitNumber);
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Street number:");
                                    bool streetNum = Int32.TryParse(Console.ReadLine(), out int streetNumber);
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Street Name:");
                                    string streetName = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Street suffix:");
                                    string streetSuffix = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("City:");
                                    string city = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("State (ACT, NSW, NT, QLD, SA, TAS, VIC, WA):");
                                    string state = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Postcode (1000 .. 9999):");
                                    bool post = Int32.TryParse(Console.ReadLine(), out int postcode);
                                    Console.WriteLine(" ");
                                    Address address = new Address(unitNumber, streetNumber, streetName, streetSuffix, city, postcode, state);
                                    if (address.checkUnit(address.unit) && address.checkStreetNumber(address.streetNumber) && address.checkStreetName(address.streetName) && address.checkCity(address.city) && address.checkPostcode(address.postcode) && address.checkState(address.state))
                                    {
                                        db.deleteAddressFile(user.email);
                                        if (db.addAddress(user.email, address))
                                        {
                                            Console.Write("Thank you for your bid. If successful, the item will be provided via delivery to " + address.unit + "/" + address.streetNumber + " " + address.streetName + " " + address.streetSuffix + ", " + address.city + " " + address.state + " " + address.postcode);
                                            clientMenu(user);
                                        }
                                    }
                                }
                            } else
                            {
                                clientMenu(user);
                            }

                        } 
                        else
                        {
                            clientMenu(user);
                        }
                    }
                    else
                    {
                        Data db = new Data();
                        if (db.checkAllProductsFile())
                        {
                            db.readSearchProductsFile(searchInput);
                            Console.WriteLine(" ");
                            Console.WriteLine("Would you like to place a bid on any of these items (yes or no)?");
                            string answer = Console.ReadLine().ToUpper();
                            if (answer == "YES")
                            {
                                Console.WriteLine(" ");
                                Console.WriteLine("Please select a product using their item number");
                                bool selected = Int32.TryParse(Console.ReadLine(), out int selectedValue);
                                db.selectedProduct(selectedValue, null, user);
                                Console.WriteLine(" ");
                                Console.WriteLine("How much do you bid?");
                                string userBid = Console.ReadLine();
                                db.selectedProduct(selectedValue, userBid, user);
                                Console.WriteLine(" ");
                                Console.WriteLine("Delivery Instructions\r\n---------------------\r\n(1) Click and collect\r\n(2) Home Delivery");
                                string selection = Console.ReadLine();
                                if (selection == "1")
                                {

                                }
                                else if (selection == "2")
                                {
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Please provide your home address.");
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Unit number (0 = none):");
                                    bool unit = Int32.TryParse(Console.ReadLine(), out int unitNumber);
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Street number:");
                                    bool streetNum = Int32.TryParse(Console.ReadLine(), out int streetNumber);
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Street Name:");
                                    string streetName = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Street suffix:");
                                    string streetSuffix = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("City:");
                                    string city = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("State (ACT, NSW, NT, QLD, SA, TAS, VIC, WA):");
                                    string state = Console.ReadLine();
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Postcode (1000 .. 9999):");
                                    bool post = Int32.TryParse(Console.ReadLine(), out int postcode);
                                    Console.WriteLine(" ");
                                    Address address = new Address(unitNumber, streetNumber, streetName, streetSuffix, city, postcode, state);
                                    if (address.checkUnit(address.unit) && address.checkStreetNumber(address.streetNumber) && address.checkStreetName(address.streetName) && address.checkCity(address.city) && address.checkPostcode(address.postcode) && address.checkState(address.state))
                                    {
                                        db.deleteAddressFile(user.email);
                                        if (db.addAddress(user.email, address))
                                        {
                                            Console.Write("Thank you for your bid. If successful, the item will be provided via delivery to " + address.unit + "/" + address.streetNumber + " " + address.streetName + " " + address.streetSuffix + ", " + address.city + " " + address.state + " " + address.postcode);
                                            clientMenu(user);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                clientMenu(user);
                            }

                        } else
                        {
                            clientMenu(user);
                        }
                    }
                }
                //PRODUCT SEARTCH END


                if (optionValue == 4)
                {
                    clientMenu(user);
                }
                if (optionValue == 5)
                {
                    clientMenu(user);
                }
                if (optionValue == 6)
                {
                    menu();
                }
            }
        }
        public static void addData(User a)
        {
            Data db = new Data();
            db.Path = "Users.txt";
            if (!db.checkEmailUniqueness(a.email))
            {
                if(db.writeFile(a))
                {
                    Console.WriteLine("Client " + a.name + "(" + a.email + " has successfully registered at the Auction House.");
                }
            }
        }
        //LOGGED IN USER FUNCTIONALITY END

        //START USER REGISTRATION VALIDATION
        public static void registration()
        {
            Data db = new Data();
            db.Path = "Users.txt";
            Console.WriteLine("Registration\n------------");
            Console.WriteLine(" ");
            Console.WriteLine("Please enter your name");
            string username = Console.ReadLine();
            bool usernameValid = checkUsername(username);
            if (usernameValid)
            {
                Console.WriteLine("Username cannot be null");
                Environment.Exit(0);
            }
            Console.WriteLine(" ");
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine();
            bool emailValid = checkEmail(email);
            if (!emailValid)
            {
                Console.WriteLine("Email Criteria has not been met");
                Environment.Exit(0);
            }
            Console.WriteLine(" ");
            Console.WriteLine("Please choose a password\n* At least 8 characters\n* No white space characters\n* At least one upper -case letter\n* At least one lower -case letter\n* At least one digit\n* At least one special character");
            string password = Console.ReadLine();
            //PASSWORD INPUT CRITERIA                        
            if (passwordCriteriaCheck(password) && !usernameValid && emailValid)
            {
                User user = new User(username, email, password);
                addData(user);
            }
        }
        public static bool checkEmail(string email)
        {
            int indexOfAt = email.IndexOf("@");
            int lastIndexOfDot = email.LastIndexOf(".");
            bool prefix = false;
            bool suffix = false;
            var validPrefix = new Regex("[A-z0-9_.-]");
            var invalidPrefixEnd = new Regex("[_.-]");
            var validSuffix = new Regex("[A-z0-9.-]");
            var letters = new Regex("[A-z]");
            if (email.Length >= 2)
            {
                if (email[0] != '@' && email[email.Length - 1] != '@')
                {
                    //check individual letters
                    for(int i = indexOfAt-1; i >= 0; i--)
                    {
                        if (validPrefix.IsMatch(email[i].ToString()))
                        {
                            if (!invalidPrefixEnd.IsMatch(email[indexOfAt - 1].ToString()))
                            {
                                prefix = true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = indexOfAt + 1; i <= email.Length - 1; i++)
                    {
                        if (validSuffix.IsMatch(email[i].ToString()) && email[i] != '_' && email.Contains(".") && email[indexOfAt + 1] != '.' && email[email.Length - 1] != '.')
                        {
                            for (int j = lastIndexOfDot + 1; i <= email.Length; i++)
                            {
                                if (letters.IsMatch(email[j].ToString()))
                                {
                                    suffix = true;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (prefix && suffix)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool checkUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return true;
            }
            return false;
        }
        public static bool passwordCriteriaCheck(string password)
        {
            var specialCharacters = new Regex("[_^\\W]+");
            if (password.Length > 7 && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit) && specialCharacters.IsMatch(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //END USER REGISTRATION VALIDATION
    }
}   