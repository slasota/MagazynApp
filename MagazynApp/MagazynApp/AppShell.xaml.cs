using MagazynApp.Views;

namespace MagazynApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
            Routing.RegisterRoute(nameof(AddEditPalletPage), typeof(AddEditPalletPage));
        }
    }
}
