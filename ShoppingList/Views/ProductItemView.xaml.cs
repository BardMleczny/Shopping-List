using Microsoft.Maui.Controls;
using ShoppingList.Models;
using System;
using System.Xml.Linq;
using System.IO;

namespace ShoppingList.Views
{
    public partial class ProductContentView : ContentView
    {
        public Action OnProductBoughtChanged { get; set; }

        private Product _product;
        public ProductContentView()
        {
            InitializeComponent();
        }

        public ProductContentView(Product product)
        {
            InitializeComponent();

            _product = product;

            ProductNameLabel.Text = product.Name;
            ProductDetailsLabel.Text = $"{product.Shop} - " + (product.IsOptional ? "optional" : "mandatory");
            BoughtCheckBox.IsChecked = product.IsBought;
            UpdateUI();
        }

        private void OnCountChanged(object sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                string countTextCopy = entry.Text;
                int index = 0;

                for (int i = 0; i < countTextCopy.Length; i++)
                {
                    if(!(countTextCopy[i] == ',' || char.IsNumber(countTextCopy[i])))
                        countTextCopy = countTextCopy.Remove(i);
                }
                if (countTextCopy.Length == 0)
                    countTextCopy = "0";
                _product.Count = float.Parse(countTextCopy);
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

            UpdateUI();
            UpdateProductInXml();


            OnProductBoughtChanged?.Invoke();
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DeleteProductFromXml();
            this.IsVisible = false;
        }

        private void UpdateUI()
        {
            string newCountText = $"{_product.Count} {_product.Unit}";
            ProductCount.Text = newCountText;

            if (_product.IsBought)
                ProductLayout.BackgroundColor = new Color(.25f);
            else
                ProductLayout.BackgroundColor = Colors.LightGray;
        }

        private void UpdateProductInXml()
        {
            var document = XDocument.Load(FileChoice.productsFilePath);
            var productElement = document.Descendants("Product")
                .FirstOrDefault(p => (string)p.Element("Name") == _product.Name && (string)p.Element("Shop") == _product.Shop);

            if (productElement != null)
            {
                productElement.SetElementValue("Value", _product.Count);
                productElement.SetElementValue("IsBought", _product.IsBought);
                document.Save(FileChoice.productsFilePath);
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
