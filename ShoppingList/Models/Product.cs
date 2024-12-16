using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class Product
    {
        public string Name { get; set; }
        public float Count { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }
        public string Shop { get; set; }
        public bool IsOptional { get; set; }
        public bool IsBought { get; set; }
        //public int Id { get; set; }

        public Product(string name, float value, string unit, string category, string shop, bool isOptional/*, int id*/)
        {
            Name = name;
            Count = value;
            Unit = unit;
            Category = category;
            Shop = shop;
            IsOptional = isOptional;
            IsBought = false;
            //Id = id;
        }

        public Product(string name, float value, string unit, string category, string shop, bool isOptional, bool isBought/*, int id*/)
        {
            Name = name;
            Count = value;
            Unit = unit;
            Category = category;
            Shop = shop;
            IsOptional = isOptional;
            IsBought = isBought;
            //Id = id;
        }
    }
}
