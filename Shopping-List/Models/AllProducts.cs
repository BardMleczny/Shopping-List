using ShoppingList.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ShoppingList.Models
{
    internal class AllProducts
    {
        public ObservableCollection<Product> Products { get; set; } = new();
        public AllProducts() => LoadProducts();

        public void LoadProducts()
        {
            Products.Clear();

            if (File.Exists(FileChoice.productsFilePath))
            {
                var doc = XDocument.Load(FileChoice.productsFilePath);
                List<Product>? allProducts = doc.Root.Element("Products")?.Elements("Product")
                    .Select(x => new Product(
                        x.Element("Name")?.Value,
                        float.Parse(x.Element("Value")?.Value ?? "0"),
                        x.Element("Unit")?.Value,
                        x.Element("Category")?.Value,
                        x.Element("Shop")?.Value,
                        bool.Parse(x.Element("IsOptional")?.Value ?? "false"),
                        bool.Parse(x.Element("IsBought")?.Value ?? "false")
                        )
                    ).ToList();

                foreach (Product product in allProducts)
                {
                    Products.Add(product);
                }
            }
        }

        public void IncreaseCount(Product product)
        {
            if (product != null)
            {
                product.Count++;
            }
        }

        public void DecreaseCount(Product product)
        {
            if (product != null && product.Count > 0)
            {
                product.Count--;
            }
        }

        public void DeleteProduct(Product product)
        {
            if (product != null && Products.Contains(product))
            {
                Products.Remove(product);
            }
        }
    }
}
