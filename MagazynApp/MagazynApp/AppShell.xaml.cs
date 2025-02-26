using MagazynApp.Views;

namespace MagazynApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DodajProduktPage), typeof(DodajProduktPage));
            Routing.RegisterRoute(nameof(AddEditPalletePage), typeof(AddEditPalletePage));
        }
    }
}
