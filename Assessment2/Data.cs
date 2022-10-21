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

        public bool readFile(string email, string password)
        {
            try
            {
                sr = new StreamReader(Path);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.Contains(email + ";" + password))
                    {
                        Console.WriteLine("RETURNED LINE " + line);
                    }
                    else
                    {
                        continue;
                    }
                }
                sr.Close();
                return true;
            }
            catch
            {
                Console.WriteLine("Cannot read file");
                return false;
            }
        }

        public bool writeFile(string username, string email, string password)
        {
            createFile();
            try
            {
                fs = new FileStream(Path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.WriteLine(username + ";" + email + ";" + password);
                sw.Close();
                fs.Close();
                return true;
            }
            catch
            {
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
