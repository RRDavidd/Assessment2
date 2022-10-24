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

        public bool readFile(string username, string email, string password)
        {
            try
            {
                sr = new StreamReader(path);
                bool found = false;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains(username + ";" + email + ";" + password))
                    {
                        Console.WriteLine("RETURNED LINE " + line);
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

        public bool addAddress()
        {
            try
            {

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool splitString(string username, string email, string password)
        {
            try
            {
                string splitThis = username + email + password;
                string[] split = splitThis.Split(';');

                foreach (string s in split)
                {
                    if (s != username + email + password)
                    {
                        Console.WriteLine(s);
                    }       
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool writeFile(string username, string email, string password)
        {
            createFile();
            try
            {
                fs = new FileStream(path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.WriteLine(username + ";" + email + ";" + password);
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
    }
}
