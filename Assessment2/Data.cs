using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
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
                    string line = sr.ReadLine();
                    int indexOf = line.IndexOf(';');
                    if (indexOf >= 0)
                    {
                        line = line.Substring(0, indexOf);
                    }
                    string finalLine = count + "       " + line;
                    addBidding(line);
                    Console.WriteLine(finalLine);
                    count++;
                }
                sr.Close();
                return true; 
            }
            catch
            {
                Console.WriteLine(" ");
                Console.WriteLine("No products to list");
                return false;
            }
        }

        //READ ALLPRODUCTS FILE WITH SEARCHING
        public bool readSearchProductsFile(string searchInput)
        {
            createBiddingFile();
            try
            {
                string clientPath = "allproducts" + ".txt";
                sr = new StreamReader(clientPath);
                int count = 1;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains(searchInput))
                    {
                        int indexOf = line.IndexOf(';');
                        if (indexOf >= 0)
                        {
                            line = line.Substring(0, indexOf);
                        }
                        string finalLine = count + "       " + line + "     ";
                        addBidding(line);
                        Console.WriteLine(finalLine);
                        count++;
                        return true;
                    }
                }
                sr.Close();
                return false;
            }
            catch
            {
                Console.WriteLine(" ");
                Console.WriteLine("No products to list");
                return false;
            }
        }

        //READ USER PRODUCTS FILE
        public bool readUserProductFile(string email)
        {
            try
            {
                string clientPath = email + "products" + ".txt";
                sr = new StreamReader(clientPath);
                int count = 1;
                while (!sr.EndOfStream)
                {
                    string line = count + "       " + sr.ReadLine();
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
        public bool selectedProduct(int selected, string? bidderAmount, User a)
        {
            try
            {
                string clientPath = "biddings" + ".txt";
                string[] lines = File.ReadAllLines(clientPath);
                if(bidderAmount == null)
                {
                    Console.WriteLine("Product selected is " + lines[selected - 1] + " and highest bid is " + "$" + bidderAmount);
                }
                string[] linesArray = File.ReadAllLines("allproducts.txt");
                int indexOf = linesArray[selected - 1].IndexOf(';');
                string input;
                if (indexOf >= 0)
                {
                    input = linesArray[selected - 1].Substring(indexOf + 1);
                    replaceString(lines[selected - 1] + " " + a.name + " " + a.email + " " + "$" + bidderAmount, input + "products.txt", selected);
                    replaceString(lines[selected - 1] + " " + a.name + " " + a.email + " " + "$" + bidderAmount, input + "allproducts.txt", selected);
                }
                return true;
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
        public bool addAllProducts(Product a, string email)
        {
            if (allProductFile())
            {
                string clientPath = "allproducts" + ".txt";
                try
                {
                    fs = new FileStream(clientPath, FileMode.Append);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(a.productName + " " + a.productDescription + " " + a.price + ";" + email);
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

        //DELETE ADDRESS FILE
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

        //CHECK IF ALLPRODUCTS IS EMPTY
        public bool checkAllProductsFile()
        {
            try
            {
                var info = new FileInfo("allproducts.txt");
                if(info.Length == 0)
                {
                    return false ;
                }
                else
                {
                    return true ;
                }
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

        //REPLACE A STRING IN A TEXTFILE
        public bool replaceString(string newText, string fileName, int line_to_edit)
        {
            try
            {
                string[] arrLine = File.ReadAllLines(fileName);
                arrLine[line_to_edit - 1] = newText;
                File.WriteAllLines(fileName, arrLine);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
