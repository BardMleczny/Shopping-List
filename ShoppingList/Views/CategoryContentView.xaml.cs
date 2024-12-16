using Microsoft.Maui.Controls;
using ShoppingList.Models;
using System.Xml.Linq;

namespace ShoppingList.Views
{
    public partial class CategoryContentView : ContentView
    {
        private Category category;
        public CategoryContentView(Category category)
        {
            InitializeComponent();
            BindingContext = category;
            this.category = category;

            //ProductsCollectionView.ItemsSource = category.Products;
            LoadProducts();
        }

        //private void ToggleExpandCollapse(object sender, EventArgs e)
        //{
        //    ProductsCollectionView.IsVisible = !ProductsCollectionView.IsVisible;
        //    (sender as Button).Text = ProductsCollectionView.IsVisible ? "⯆" : "⯈";
        //}

        void LoadProducts()
        {

            XDocument xDocument = XDocument.Load(FileChoice.productsFilePath);

            List<Product> products = xDocument.Descendants("Product")
                                    .Select(product => new Product(
                                        name: product.Element("Name")?.Value,
                                        value: float.Parse(product.Element("Value")?.Value ?? "0"),
                                        unit: product.Element("Unit")?.Value,
                                        category: product.Element("Category")?.Value,
                                        shop: product.Element("Shop")?.Value,
                                        isOptional: bool.Parse(product.Element("IsOptional")?.Value ?? "false"),
                                        isBought: bool.Parse(product.Element("IsBought")?.Value ?? "false")
                                    ))
                                    .Where(c => c.Category == category.Name)
                                    .ToList();

            products.ForEach(product =>
            {
                ProductContentView productContentView = new ProductContentView(product);
                CategoryStackLayout.Children.Add(productContentView);
            });
        }
    }
}
