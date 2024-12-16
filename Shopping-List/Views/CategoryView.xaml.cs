using ShoppingList.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace ShoppingList.Views;

public partial class CategoryView : ContentPage
{
    public ObservableCollection<GroupedProduct> GroupedProducts { get; private set; } = new();

    public CategoryView()
    {
        InitializeComponent();
        LoadProductsFromXml();
    }

    private void LoadProductsFromXml()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "products.xml");

        if (!File.Exists(filePath))
            return;

        // Parse XML
        var xml = XDocument.Load(filePath);
        var products = xml.Descendants("Product")
                          .Select(p => new Product
                          {
                              Name = (string)p.Element("Name"),
                              Unit = (string)p.Element("Unit"),
                              Category = (string)p.Element("Category"),
                              IsOptional = (bool)p.Element("IsOptional"),
                              IsBought = (bool)p.Element("IsBought"),
                              Count = (int)p.Element("Count"),
                          }).ToList();

        // Group products by Category
        var grouped = products
            .GroupBy(p => p.Category)
            .Select(g => new GroupedProduct { Name = g.Key, Items = new ObservableCollection<Product>(g) });

        GroupedProducts.Clear();
        foreach (var group in grouped)
        {
            GroupedProducts.Add(group);
        }

        GroupedProductsCollectionView.ItemsSource = GroupedProducts;
    }
}

public class GroupedProduct
{
    public string Name { get; set; }
    public ObservableCollection<Product> Items { get; set; }
}
}