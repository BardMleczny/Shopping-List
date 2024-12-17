using System.Xml.Linq;
using CommunityToolkit.Maui.Storage;

namespace ShoppingList.Views
{
    public partial class AllProducts : ContentPage
    {
        public AllProducts()
        {
            InitializeComponent();
        }

        private async void ExportClicked(object sender, EventArgs e)
        {
            var folderResult = await FolderPicker.Default.PickAsync();

            if (folderResult.IsSuccessful)
            {
                var folderPath = folderResult.Folder.Path;

                string fileName = "ProductsExport.xml";
                string filePath = Path.Combine(folderPath, fileName);

                XDocument.Load(FileChoice.productsFilePath).Save(filePath);

                await DisplayAlert("Export Successful", $"XML file saved to: {filePath}", "OK");
            }
            else
            {
                await DisplayAlert("Export Canceled", "No folder selected.", "OK");
            }
        }

        private async void AddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddProduct));
        }
        private async void OnToBuyClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProductsToBuy));
        }
        private async void OnShopClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProductsFromShop));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadCategories();
        }


        private void LoadCategories()
        {

            var categories = XDocument.Load(FileChoice.dataFilePath).Descendants("Category")
                                                                    .Select(category => category.Value)
                                                                    .Where(c => c != "+")
                                                                    .ToList();

            CategoriesStackLayout.Children.Clear();

            foreach (var category in categories)
            {
                CategoryContentView categoryView = new (new (category));
                CategoriesStackLayout.Children.Add(categoryView);
            }
        }
    }
}
