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
            dataFilePath = Path.Combine(Path.Combine(FileSystem.AppDataDirectory, "ShoppingList.Data.xml"));
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

                    productsFilePath = fileResult.FullPath;

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
