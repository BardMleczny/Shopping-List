namespace ShoppingList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.AllProducts), typeof(Views.AllProducts));
            Routing.RegisterRoute(nameof(Views.AddProduct), typeof(Views.AddProduct));
        }
    }
}
