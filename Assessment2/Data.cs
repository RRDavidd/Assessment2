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

        public bool writeFile(string username, string email, string password)
        {
            Console.WriteLine("file creation:" + createFile());
            try
            {
                fs = new FileStream(Path, FileMode.Append);
                sw = new StreamWriter(fs);
                sw.WriteLine(username + email + password);
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
