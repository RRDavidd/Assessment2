using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Assessment2
{
    class Data
    {
        private StreamWriter sw = null;
        private StreamReader sr = null;
        private FileStream fs = null;

        private string path = "";

        public string Path { 
            get { return path; } 
            set { path = value;} 
        }

        //CHECK IF EMAIL PASSED IS UNIQUE TO OTHER USER'S EMAILS
        public bool checkEmailUniqueness(string email)
        {
            try
            {
                fs = new FileStream(path, FileMode.Open);
                sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains(email))
                    {
                        Console.WriteLine("Email is not unique");
                        return true;
                    }
                }
                fs.Close();
                sr.Close();
                return false;
            }
            catch
            {
                return false;
            }
        }

        //READ FILE FOR USER PARAMETER AND CHECK IF IT EXISTS
        public bool readFile(User a)
        {
            try
            {
                sr = new StreamReader(path);
                bool found = false;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains(a.name + ";" + a.email + ";" + a.password))
                    {
                        found = true;
                        return true;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Cannot find user");
                    return false;
                }
                sr.Close();
                return false;
            }
            catch
            {
                Console.WriteLine("Cannot read file");
                return false;
            }
        }

        //READ ALL PRODUCTS FILE
        public bool readAllProductsFile(string? bidderName, string? bidderEmail, string? bidderAmount)
        {
            createBiddingFile();
            try
            {
                string clientPath = "allproducts" + ".txt";
                sr = new StreamReader(clientPath);
                int count = 1;
                if(bidderName == null)
                {
                    bidderName = "-";
                }
                if (bidderEmail == null)
                {
                    bidderEmail = "-";
                }
                if (bidderAmount == null)
                {
                    bidderAmount = "-";
                }
                while (!sr.EndOfStream)
                {
                    string line = count + "       " + sr.ReadLine() + "     " + bidderName + "     " + bidderEmail + "      " + bidderAmount;
                    addBidding(line);
                    Console.WriteLine(line);
                    count++;
                }
                sr.Close();
                return false;
            }
            catch
            {
                Console.WriteLine("Cannot read file");
                return false;
            }
        }

        //READ USER PRODUCTS FILE
        public bool readUserProductFile(string email, string? bidderName, string? bidderEmail, string? bidderAmount)
        {
            try
            {
                string clientPath = email + "products" + ".txt";
                sr = new StreamReader(clientPath);
                int count = 1;
                if (bidderName == null)
                {
                    bidderName = "-";
                }
                if (bidderEmail == null)
                {
                    bidderEmail = "-";
                }
                if (bidderAmount == null)
                {
                    bidderAmount = "-";
                }
                while (!sr.EndOfStream)
                {
                    string line = count + "       " + sr.ReadLine() + "     " + bidderName + "     " + bidderEmail + "      " + bidderAmount;
                    addBidding(line);
                    Console.WriteLine(line);
                    count++;
                }
                sr.Close();
                return false;
            }
            catch
            {
                Console.WriteLine("No products for this user");
                return false;
            }
        }

        //SELECT PRODUCT TO BID
        public bool selectedProduct(string selected, string? bidderAmount)
        {
            try
            {
                string clientPath = "biddings" + ".txt";
                sr = new StreamReader(clientPath);
                char trim = '-';
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if(line[0].ToString() == selected)
                    {
                        if(bidderAmount == null)
                        {
                            Console.WriteLine("Bidding for " + line + ", current highest bid " + bidderAmount);
                        }
                        else
                        {
                            line.Trim(trim);
                            Console.WriteLine("Your bid of " + "$" + bidderAmount + " for " + line + " is placed.");
                        }
                        return true;
                    }
                }
                sr.Close();
                return false;
            }
            catch
            {
                Console.WriteLine("Cannot read file");
                return false;
            }
        }

        //FIRST TIME INPUT ADDRESS FOR USER
        public bool addAddress(string email,Address a)
        {
            if (clientAddressFile(email))
            {
                string clientPath = email + "address" + ".txt";
                try
                {
                    fs = new FileStream(clientPath, FileMode.Append);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(a.unit + "/" + a.streetNumber + " " + a.streetName + " " + a.streetSuffix + " " + a.city + " " + a.state + " " + a.postcode);
                    sw.Close();
                    fs.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        //ADD BIDDING
        public bool addBidding(string bidding)
        {
            {
                string clientPath = "biddings" + ".txt";
                try
                {
                    fs = new FileStream(clientPath, FileMode.Append);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(bidding);
                    sw.Close();
                    fs.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        
        //ADD PRODUCTS
        public bool addProducts(string email, Product a)
        {
            if (clientProductFile(email))
            {
                string clientPath = email + "products" + ".txt";
                try
                {
                    fs = new FileStream(clientPath, FileMode.Append);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(a.productName + " " + a.productDescription + " " + a.price);
                    sw.Close();
                    fs.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        //CREATE PRODUCT FILE FOR ALL REGISTERED ADVERTISED PRODUCTS
        public bool addAllProducts(Product a)
        {
            if (allProductFile())
            {
                string clientPath = "allproducts" + ".txt";
                try
                {
                    fs = new FileStream(clientPath, FileMode.Append);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(a.productName + " " + a.productDescription + " " + a.price);
                    sw.Close();
                    fs.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        //WRITEFILE FOR USER
        public bool writeFile(User a)
        {
            createFile();
            try
            {
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.WriteLine(a.name + ";" + a.email + ";" + a.password);
                sw.Close();
                fs.Close();
                return true;
            }
            catch
            {
                Console.WriteLine("Cannot write on file, please restart program");
                return false;
            }
        }

        //CREATE USER FILE AND CHECK IF ALREADY EXISTS
        private bool createFile()
        {
            try
            {
                if (!File.Exists(path))
                {
                    fs = new FileStream(path, FileMode.CreateNew);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        //CREATE PERSONAL CLIENT ADDRESS FILE 
        private bool clientAddressFile(string email)
        {
            try
            {
                string clientPath = email + "address" + ".txt";
                if (!File.Exists(clientPath))
                {
                    fs = new FileStream(clientPath, FileMode.CreateNew);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        //CREATE PERSONAL CLIENT PRODUCT FILE
        public bool clientProductFile(string email)
        {
            try
            {
                string clientPath = email + "products" + ".txt";
                if (!File.Exists(clientPath))
                {
                    fs = new FileStream(clientPath, FileMode.CreateNew);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //CREATE BIDDING FILE
        private bool createBiddingFile()
        {
            try
            {
                string clientPath = "biddings" + ".txt";
                File.Delete(clientPath);
                if (!File.Exists(clientPath))
                {
                    fs = new FileStream(clientPath, FileMode.CreateNew);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //CREATE ALL PRODUCTS FILE
        public bool allProductFile()
        {
            try
            {
                string clientPath = "allproducts" + ".txt";
                if (!File.Exists(clientPath))
                {
                    fs = new FileStream(clientPath, FileMode.CreateNew);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteAddressFile(string email)
        {
            try
            {
                File.Delete(email + "address.txt");
                return true;
            }
            catch
            {
                return false;
            }
        }

        //CHECK IF CLIENT PRODUCT FILE ALREADY EXISTS
        public bool checkClientProductFile(string email)
        {
            string clientPath = email + "products" +".txt";
            if (!File.Exists(clientPath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //CHECK IF CLIENT ADDRESS FILE ALREADY EXISTS
        public bool checkClientAddressFile(string email)
        {
            string clientPath = email + "address" + ".txt";
            if (!File.Exists(clientPath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
