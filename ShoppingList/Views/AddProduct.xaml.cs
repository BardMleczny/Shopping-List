using Microsoft.Maui.Controls;
using ShoppingList.Models;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ShoppingList.Views
{
    public partial class AddProduct : ContentPage
    {
        public AddProduct()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            if (!File.Exists(FileChoice.dataFilePath))
            {
                var initialDoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Data",
                        new XElement("Units",
                            new XElement("Unit", "psc"),
                            new XElement("Unit", "kg"),
                            new XElement("Unit", "l"),
                            new XElement("Unit", "+")
                        ),
                        new XElement("Categories",
                            new XElement("Category", "diary"),
                            new XElement("Category", "meat"),
                            new XElement("Category", "fruits"),
                            new XElement("Category", "vegetables"),
                            new XElement("Category", "+")
                        ),
                        new XElement("Shops",
                            new XElement("Shop", "Biedronka"),
                            new XElement("Shop", "Lidl"),
                            new XElement("Shop", "+")
                        )
                    )
                );

                initialDoc.Save(FileChoice.dataFilePath);
            }

            var doc = XDocument.Load(FileChoice.dataFilePath);
            var units = doc.Root.Descendants("Unit")
                                .Select(x => x.Value)
                                .ToList();
            if (units != null)
                UnitPicker.ItemsSource = units;

            var categories = doc.Root.Descendants("Category")
                                .Select(x => x.Value)
                                .ToList();
            if (categories != null)
                CategoryPicker.ItemsSource = categories;

            var shops = doc.Root.Descendants("Shop")
                                .Select(x => x.Value)
                                .ToList();
            if (shops != null)
                ShopPicker.ItemsSource = shops;

        }

        private async void AddNewItem(string category, string newItem)
        {
            var doc = XDocument.Load(FileChoice.dataFilePath);

            if (category == "Unit")
            {
                doc.Root.Element("Units").Elements().ToList()[^1].AddBeforeSelf(new XElement("Unit", newItem));
            }
            else if (category == "Category")
            {
                doc.Root.Element("Categories").Elements().ToList()[^1].AddBeforeSelf(new XElement("Category", newItem));
            }
            else if (category == "Shop")
            {
                doc.Root.Element("Shops").Elements().ToList()[^1].AddBeforeSelf(new XElement("Shop", newItem));
            }

            doc.Save(FileChoice.dataFilePath);
            LoadData();
        }

        private async void OnUnitChanged(object sender, EventArgs e)
        {
            if (UnitPicker.SelectedItem?.ToString() == "+")
            {
                string newUnit = await DisplayPromptAsync("Add Unit", "Enter a new unit:");
                if (!string.IsNullOrWhiteSpace(newUnit))
                {
                    AddNewItem("Unit", newUnit);
                }
            }
        }

        private async void OnCategoryChanged(object sender, EventArgs e)
        {
            if (CategoryPicker.SelectedItem?.ToString() == "+")
            {
                string newCategory = await DisplayPromptAsync("Add Category", "Enter a new category:");
                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    AddNewItem("Category", newCategory);
                }
            }
        }

        private async void OnShopChanged(object sender, EventArgs e)
        {
            if (ShopPicker.SelectedItem?.ToString() == "+")
            {
                string newShop = await DisplayPromptAsync("Add Shop", "Enter a new shop:");
                if (!string.IsNullOrWhiteSpace(newShop))
                {
                    AddNewItem("Shop", newShop);
                }
            }
        }

        private void OnValueChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Editor entry)
            {
                int index = 0;
                foreach (char c in entry.Text)
                {
                    if (char.IsWhiteSpace(c))
                        entry.Text = entry.Text.Remove(index, 1);
                    index++;
                }
            }
        }
        private void OnNumberValueChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                int index = 0;
                foreach (char c in entry.Text)
                {
                    int dotCount = 0;
                    foreach (char stringIterator in entry.Text)
                    {
                        if (stringIterator == ',')
                            dotCount++;
                    }
                    if (dotCount >= 2)
                    {
                        if (!char.IsDigit(c))
                            entry.Text = entry.Text.Remove(index, 1);
                    }
                    else
                    {
                        if (!char.IsDigit(c) && c != ',')
                            entry.Text = entry.Text.Remove(index, 1);
                    }
                        
                    index++;
                }
            }
        }

        private void OnIncreaseClicked(object sender, EventArgs e)
        {
            if (int.TryParse(CounterValue.Text, out int count))
            {
                CounterValue.Text = (count + 1).ToString();
            }
            else
            {
                CounterValue.Text = "1";
            }
        }

        private void OnDecreaseClicked(object sender, EventArgs e)
        {
            if (int.TryParse(CounterValue.Text, out int count) && count > 0)
            {
                CounterValue.Text = (count - 1).ToString();
            }
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductName.Text) ||
                string.IsNullOrWhiteSpace(CounterValue.Text) ||
                UnitPicker.SelectedItem == null || UnitPicker.SelectedItem.ToString() == "+" ||
                CategoryPicker.SelectedItem == null || CategoryPicker.SelectedItem.ToString() == "+" ||
                ShopPicker.SelectedItem == null || ShopPicker.SelectedItem.ToString() == "+")
            {
                ErrorMessage.IsVisible = true;
                return;
            }

            ErrorMessage.IsVisible = false;

            var product = new Product(
                ProductName.Text,
                float.Parse(CounterValue.Text),
                UnitPicker.SelectedItem.ToString(),
                CategoryPicker.SelectedItem.ToString(),
                ShopPicker.SelectedItem.ToString(),
                OptionalCheckBox.IsChecked
            );
            if (!File.Exists(FileChoice.productsFilePath))
            {
                var initialDoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Data",
                        new XElement("Products")
                    )
                );

                initialDoc.Save(FileChoice.productsFilePath);
            }

            var doc = XDocument.Load(FileChoice.productsFilePath);

            var productElement = new XElement("Product",
                new XElement("Name", product.Name),
                new XElement("Value", product.Count),
                new XElement("Unit", product.Unit),
                new XElement("Category", product.Category),
                new XElement("Shop", product.Shop),
                new XElement("IsOptional", product.IsOptional),
                new XElement("IsBought", product.IsBought)
            );

            doc.Root.Element("Products")?.Add(productElement);

            doc.Save(FileChoice.productsFilePath);

            ProductName.Text = string.Empty;
            CounterValue.Text = "";
            UnitPicker.SelectedIndex = -1;
            ShopPicker.SelectedIndex = -1;

            DisplayAlert("Success", "Product added successfully!", "OK");
        }

    }
}
