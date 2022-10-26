using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment2
{
    class User
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public User(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
        }

    }

 
}
