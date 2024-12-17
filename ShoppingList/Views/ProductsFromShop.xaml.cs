using System.Xml.Linq;

using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class ProductsFromShop : ContentPage
{
	public ProductsFromShop()
    {
        InitializeComponent();
        LoadShopSelection();
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddProduct));
    }

    private void OnFilterClicked(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void LoadShopSelection()
    {
        var shops = XDocument.Load(FileChoice.dataFilePath).Root.Descendants("Shop")
                                                                .Select(shop => shop.Value)
                                                                .Where(c => c != "+")
                                                                .ToList();
        if (shops != null)
            ShopPicker.ItemsSource = shops;
    }

    public void LoadProducts()
    {
        string shop = ShopPicker.SelectedItem == null ? "" : ShopPicker.SelectedItem.ToString();

        var products = XDocument.Load(FileChoice.productsFilePath).Descendants("Product")
                                                                    .Select(product => new Product(
                                                                        name: product.Element("Name")?.Value,
                                                                        value: float.Parse(product.Element("Value")?.Value ?? "0"),
                                                                        unit: product.Element("Unit")?.Value,
                                                                        category: product.Element("Category")?.Value,
                                                                        shop: product.Element("Shop")?.Value,
                                                                        isOptional: bool.Parse(product.Element("IsOptional")?.Value ?? "false"),
                                                                        isBought: bool.Parse(product.Element("IsBought")?.Value ?? "false")
                                                                    ))
                                                                    .Where(c => c.Shop == shop)
                                                                    .OrderBy(p => p.Category)
                                                                    .ToList();

        ProductsStackLayout.Children.Clear();

        foreach (Product product in products)
        {
            ProductContentView productView = new (product);
            ProductsStackLayout.Children.Add(productView);
        }
    }
}