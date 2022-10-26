using System;
using System.ComponentModel.Design;
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
                if (db.checkClientFile(email))
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
        public static void clientMenu(User a)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Client Menu\r\n-----------\r\n(1) Advertise Product\r\n(2) View My Product List\r\n(3) Search For Advertised Products\r\n(4) View Bids On My Products\r\n(5) View My Purchased Items\r\n(6) Log off ");
            Console.WriteLine(" ");
            Console.WriteLine("Choose from 1-6");
            bool option = Int32.TryParse(Console.ReadLine(), out int optionValue);
            if (option)
            {
                if(optionValue == 1)
                {
                    Console.WriteLine("1");
                }
                if (optionValue == 2)
                {
                    Console.WriteLine("2");
                }
                if (optionValue == 3)
                {
                    Console.WriteLine("3");
                }
                if (optionValue == 4)
                {
                    Console.WriteLine("4");
                }
                if (optionValue == 5)
                {
                    Console.WriteLine("5");
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
}    //@joie: I LOVE RR DABED <3 