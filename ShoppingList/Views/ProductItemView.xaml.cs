using Microsoft.Maui.Controls;
using ShoppingList.Models;
using System;
using System.Xml.Linq;
using System.IO;

namespace ShoppingList.Views
{
    public partial class ProductContentView : ContentView
    {
        string oldCountText = "";
        private Product _product;
        public ProductContentView()
        {
            InitializeComponent();
            OnCountChanged(ProductCount, null);
        }

        public ProductContentView(Product product)
        {
            InitializeComponent();

            _product = product;

            ProductNameLabel.Text = product.Name;
            ProductDetailsLabel.Text = $"{product.Shop} - " + (product.IsOptional ? "opcjonalny" : "obowi¹zkowy");
            ProductCount.Text = $"{product.Count} {product.Unit}";
            BoughtCheckBox.IsChecked = product.IsBought;
        }

        private void OnCountChanged(object sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                int index = 0;
                foreach (char c in entry.Text)
                {
                    int dotCount = 0;
                    foreach (char stringIterator in entry.Text)
                    {
                        if (stringIterator == '.')
                            dotCount++;
                    }
                    if (dotCount >= 2)
                    {
                        if (!char.IsDigit(c))
                            entry.Text = entry.Text.Remove(index, 1);
                    }
                    else
                    {
                        if (!char.IsDigit(c) && c != '.')
                            entry.Text = entry.Text.Remove(index, 1);
                    }

                    index++;
                }
                
                _product.Count = float.Parse(string.Concat(entry.Text.Where(char.IsDigit)));

                string newCountText = $"{_product.Count} {_product.Unit}";
                if (newCountText != oldCountText)
                {
                    oldCountText = newCountText;
                    ProductCount.Text = newCountText;
                }
            }

            UpdateProductInXml();
        }

        private void OnIncreaseClicked(object sender, EventArgs e)
        {
            _product.Count += 1;
            UpdateUI();
            UpdateProductInXml();
        }

        private void OnDecreaseClicked(object sender, EventArgs e)
        {
            if (_product.Count > 0)
            {
                _product.Count -= 1;
                UpdateUI();
                UpdateProductInXml();
            }
        }

        private void OnBoughtChanged(object sender, CheckedChangedEventArgs e)
        {
            _product.IsBought = e.Value;
            UpdateProductInXml();
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DeleteProductFromXml();
            this.IsVisible = false;
        }

        private void UpdateUI()
        {
            string newCountText = $"{_product.Count} {_product.Unit}";
            oldCountText = newCountText;
            ProductCount.Text = newCountText;
        }

        private void UpdateProductInXml()
        {
            try
            {
                var document = XDocument.Load(FileChoice.productsFilePath);

                var productElement = document.Root.Elements("Product")
                    .FirstOrDefault(p => (string)p.Element("Name") == _product.Name && (string)p.Element("Shop") == _product.Shop);

                if (productElement != null)
                {
                    productElement.SetElementValue("Value", _product.Count);
                    productElement.SetElementValue("IsBought", _product.IsBought);
                    document.Save(FileChoice.productsFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product in XML: {ex.Message}");
            }
        }

        private void DeleteProductFromXml()
        {
            try
            {
                var document = XDocument.Load(FileChoice.productsFilePath);

                var productElement = document.Root.Elements("Product")
                    .FirstOrDefault(p => (string)p.Element("Name") == _product.Name && (string)p.Element("Shop") == _product.Shop);

                if (productElement != null)
                {
                    productElement.Remove();
                    document.Save(FileChoice.productsFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product from XML: {ex.Message}");
            }
        }
    }
}
