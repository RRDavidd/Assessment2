using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Assessment2
{
    class Program
    {
        public static void Main(string[] args)
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
                            Registration();
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


        public static void Registration()
        {
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
                Console.WriteLine("Registration Complete!");
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
    }
}