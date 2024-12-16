using ShoppingList.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ShoppingList.Views
{
    public partial class Products : ContentPage
    {

        public Products()
        {
            InitializeComponent();
            BindingContext = new Models.AllProducts();

            var doc = XDocument.Load(FileChoice.dataFilePath);
            var shops = doc.Root.Element("Shops")
                                ?.Elements("Shop")
                                .Select(x => x.Value)
                                .ToList();
            if (shops != null)
                ShopFilterPicker.ItemsSource = shops;
            ApplyFilters();
        }

        private async void AddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddProduct));
        }

        private void ApplyFilters()
        {
            IEnumerable<Product> filteredProducts = ((Models.AllProducts)BindingContext).Products;

            // Filter by shop
            if (ShopFilterPicker.SelectedItem is string selectedShop && !string.IsNullOrWhiteSpace(selectedShop))
            {
                filteredProducts = filteredProducts.Where(p => p.Shop == selectedShop);
            }

            // Filter to show only not-bought products
            if (NotBoughtSwitch.IsToggled)
            {
                filteredProducts = filteredProducts.Where(p => !p.IsBought);

                // Don't group by category, show as a flat list
                ProductsCollectionView.IsGrouped = false;
                ProductsCollectionView.ItemsSource = filteredProducts.ToList();
            }
            else
            {
                // Group by category
                var groupedProducts = filteredProducts
                    .GroupBy(p => p.Category)
                    .Select(g => new ProductGroup<Product>(g.Key, g))
                    .ToList();

                if (ProductsCollectionView != null)
                {
                    //ProductsCollectionView.IsGrouped = true;
                    //ProductsCollectionView.ItemsSource = groupedProducts;
                }


            }
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private async void PlusClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddProduct));
        }

        private async void MinusClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddProduct));
        }
    }
}
