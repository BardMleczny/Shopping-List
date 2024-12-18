using System.Xml.Linq;

using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class ProductsToBuy : ContentPage
{
	public ProductsToBuy()
    {
        InitializeComponent();
        LoadProducts();
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddProduct));
    }

    private void OnSortingChanged(object sender, EventArgs e)
    {
        LoadProducts();
    }

    public void LoadProducts()
    {

        var unSortedProducts = XDocument.Load(FileChoice.productsFilePath).Descendants("Product")
                                                                    .Select(product => new Product(
                                                                        name: product.Element("Name")?.Value,
                                                                        value: float.Parse(product.Element("Value")?.Value ?? "0"),
                                                                        unit: product.Element("Unit")?.Value,
                                                                        category: product.Element("Category")?.Value,
                                                                        shop: product.Element("Shop")?.Value,
                                                                        isOptional: bool.Parse(product.Element("IsOptional")?.Value ?? "false"),
                                                                        isBought: bool.Parse(product.Element("IsBought")?.Value ?? "false")
                                                                    ))
                                                                    .Where(c => c.IsBought == false);

        var sortedProducts = unSortedProducts;

        switch (SortingPicker.SelectedIndex)
        {
            case 0:
                sortedProducts = unSortedProducts.OrderBy(c => c.Category);
                break;
            case 1:
                sortedProducts = unSortedProducts.OrderBy(c => c.Name);
                break;
            case 2:
                sortedProducts = unSortedProducts.OrderBy(c => c.Count);
                break;
        }

        var products = sortedProducts.ToList();

        ProductsStackLayout.Children.Clear();

        foreach (Product product in products)
        {
            ProductContentView productView = new (product);
            productView.OnProductBoughtChanged = LoadProducts;
            ProductsStackLayout.Children.Add(productView);
        }
    }
}