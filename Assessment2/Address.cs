using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Assessment2
{
    class Address
    {
        public int unit;
        public int streetNumber;
        public string streetName;
        public string streetSuffix;
        public string city;
        public int postcode;
        public string state;

        public Address(int unit, int streetNumber, string streetName,string streetSuffix, string city, int postcode, string state)
        {
            this.unit = unit;
            this.streetNumber = streetNumber;
            this.streetName = streetName;
            this.streetSuffix = streetSuffix;
            this.city = city;
            this.postcode = postcode;
            this.state = state;
        }
        public bool checkUnit(int unit)
        {
            if(unit >= 0)
            {
                return true;
            } else
            {
                Console.WriteLine("Invalid Unit");
                return false;
            }
        }

        public bool checkStreetNumber(int streetNumber)
        {
            if (streetNumber > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid Street Number");
                return false;
            }
        }

        public bool checkStreetName(string streetName)
        {
            if (streetName != null)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid Street Name");
                return false;
            }
        }

        public bool checkCity(string city)
        {
            if(city != null)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid City");
                return false;
            }
        }

        public bool checkPostcode(int postcode)
        {
            if (postcode >= 1000 && postcode <= 9999)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid Postcode");
                return false;
            }
        }

        public bool checkState(string state)
        {
            if (state.ToUpper().Equals("QLD") || state.ToUpper().Equals("NSW") || state.ToUpper().Equals("VIC") || state.ToUpper().Equals("TAS") || state.ToUpper().Equals("SA") || state.ToUpper().Equals("WA") || state.ToUpper().Equals("NT") || state.ToUpper().Equals("ACT"))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid State");
                return false;
            }
        }

    }
}
