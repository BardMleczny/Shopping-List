using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Xml.Linq;

using ShoppingList;

namespace ShoppingList.Views
{
    public partial class FileChoice : ContentPage
    {
        public static string productsFilePath { get; private set; }
        public static string dataFilePath { get; private set; }

        public FileChoice()
        {
            InitializeComponent();
            PrepareDataFile();
        }

        private async void PrepareDataFile()
        {
            dataFilePath = Path.Combine(Path.Combine(FileSystem.AppDataDirectory, "ShoppingList.Data.xml"));

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
                            new XElement("Shop", "N/A"),
                            new XElement("Shop", "Biedronka"),
                            new XElement("Shop", "Lidl"),
                            new XElement("Shop", "+")
                        )
                    )
                );

                initialDoc.Save(FileChoice.dataFilePath);
            }
        }

        private async void ShowProducts()
        {
            await Shell.Current.GoToAsync(nameof(AllProducts));
        }

        private async void OnUseBaseXmlsClicked(object sender, EventArgs e)
        {
            productsFilePath = Path.Combine(FileSystem.AppDataDirectory, "ShoppingList.Products.xml");

            if (!File.Exists(productsFilePath))
            {
                var initialDoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Data",
                        new XElement("Products")
                    )
                );

                initialDoc.Save(productsFilePath);
            }

            ShowProducts();
        }

        private async void OnImportProductsXmlClicked(object sender, EventArgs e)
        {
            try
            {
                var fileResult = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Products XML"
                });

                if (fileResult != null)
                {
                    if (Path.GetExtension(fileResult.FullPath)?.ToLower() != ".xml")
                    {
                        await DisplayAlert("Error", "Please select a valid XML file.", "OK");
                        return;
                    }
                    
                    var importedDoc = XDocument.Load(fileResult.FullPath);

                    productsFilePath = fileResult.FullPath;
                    
                    var dataDoc = XDocument.Load(dataFilePath);

                    var existingUnits = new HashSet<string>(dataDoc.Root.Element("Units")?.Elements("Unit").Select(e => e.Value) ?? Enumerable.Empty<string>());
                    var existingCategories = new HashSet<string>(dataDoc.Root.Element("Categories")?.Elements("Category").Select(e => e.Value) ?? Enumerable.Empty<string>());
                    var existingShops = new HashSet<string>(dataDoc.Root.Element("Shops")?.Elements("Shop").Select(e => e.Value) ?? Enumerable.Empty<string>());

                    var newUnits = importedDoc.Root.Element("Products")?.Elements("Product")
                        .Select(e => e.Element("Unit")?.Value)
                        .Where(v => !string.IsNullOrEmpty(v) && !existingUnits.Contains(v)) ?? Enumerable.Empty<string>();

                    var newCategories = importedDoc.Root.Element("Products")?.Elements("Product")
                        .Select(e => e.Element("Category")?.Value)
                        .Where(v => !string.IsNullOrEmpty(v) && !existingCategories.Contains(v)) ?? Enumerable.Empty<string>();

                    var newShops = importedDoc.Root.Element("Products")?.Elements("Product")
                        .Select(e => e.Element("Shop")?.Value)
                        .Where(v => !string.IsNullOrEmpty(v) && !existingShops.Contains(v)) ?? Enumerable.Empty<string>();

                    // Add the new elements to the data file
                    dataDoc.Root.Element("Units").Elements().ToList()[^1].AddBeforeSelf(newUnits.Select(u => new XElement("Unit", u)));
                    dataDoc.Root.Element("Categories").Elements().ToList()[^1].AddBeforeSelf(newCategories.Select(c => new XElement("Category", c)));
                    dataDoc.Root.Element("Shops").Elements().ToList()[^1].AddBeforeSelf(newShops.Select(s => new XElement("Shop", s)));

                    dataDoc.Save(dataFilePath);

                    ShowProducts();
                }
                else
                {
                    await DisplayAlert("Cancelled", "No file was selected.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

    }
}
