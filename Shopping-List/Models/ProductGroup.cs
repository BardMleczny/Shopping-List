using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class ProductGroup<Product> : List<Product>
    {
        public string Name { get; private set; }

        public ProductGroup(string name, IEnumerable<Product> items) : base(items)
        {
            Name = name;
        }
    }

}
