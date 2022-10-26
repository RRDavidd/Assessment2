using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool addAddress(string email,Address a)
        {
            if (clientFile(email))
            {
                string clientPath = email + ".txt";
                try
                {
                    fs = new FileStream(clientPath, FileMode.Append);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(a.unit + " " + a.streetNumber + " " + a.streetName + " " + a.streetSuffix + " " + a.city + " " + a.state + " " + a.postcode);
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

        private bool clientFile(string email)
        {
            try
            {
                string clientPath = email + ".txt";
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
        public bool checkClientFile(string email)
        {
            string clientPath = email + ".txt";
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
