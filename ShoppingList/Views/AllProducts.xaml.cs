using System.Xml.Linq;

namespace ShoppingList.Views
{
    public partial class AllProducts : ContentPage
    {
        public AllProducts()
        {
            InitializeComponent();
            LoadCategories();
        }

        private async void AddClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddProduct));
        }

        private void LoadCategories()
        {

            var categories = XDocument.Load(FileChoice.dataFilePath).Descendants("Category")
                                                                    .Select(category => category.Value)
                                                                    .Where(c => c != "+")
                                                                    .ToList();

            foreach (var category in categories)
            {
                CategoryContentView categoryView = new CategoryContentView(new (category));
                CategoriesStackLayout.Children.Add(categoryView);
            }
        }
    }
}
