namespace ShoppingList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.AllProducts), typeof(Views.AllProducts));
            Routing.RegisterRoute(nameof(Views.ProductsToBuy), typeof(Views.ProductsToBuy));
            Routing.RegisterRoute(nameof(Views.ProductsFromShop), typeof(Views.ProductsFromShop));
            Routing.RegisterRoute(nameof(Views.AddProduct), typeof(Views.AddProduct));
        }
    }
}
