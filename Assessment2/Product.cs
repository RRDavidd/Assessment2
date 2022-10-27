using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment2
{
    class Product
    {
        public string productName;
        public string productDescription;
        public string price;

        public Product(string productName, string productDescription, string price)
        {
            this.productName = productName;
            this.productDescription = productDescription;
            this.price = price;
        }

        public bool checkProductName(string productName)
        {
            if (productName != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkProductDescription(string productDescription)
        {
            if (productDescription != null && productDescription != productName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkPrice(string price)
        {
            if (price[0] != '$')
            {
                Console.WriteLine("Invalid Price");
                return false;
            } else if(price[price.IndexOf('.') + 3] != ' ')
            {
                Console.WriteLine("Invalid Price");
                return false;
            } else
            {
                return true;
            }
        }
    }
}
