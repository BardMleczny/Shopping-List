using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShoppingList.Models
{
    internal class ProductContentView
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public bool IsOptional { get; set; }
        public bool IsBought { get; set; }
        public int Count { get; set; }

        public ICommand IncreaseCountCommand { get; set; }
        public ICommand DecreaseCountCommand { get; set; }

        public ICommand DeleteCommand { get; set; }
    }
}
