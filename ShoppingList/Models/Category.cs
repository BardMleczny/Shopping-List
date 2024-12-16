using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class Category(string name)
    {
        public string Name { get; set; } = name;
        public List<Product> Products { get; set; } = new List<Product>();
    }

}
