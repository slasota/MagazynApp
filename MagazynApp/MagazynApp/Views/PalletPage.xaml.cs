using System.Threading.Tasks;
using MagazynApp.ViewModels;

namespace MagazynApp.Views;

public partial class PalletPage : ContentPage
{
	public PalletPage(PalletesViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if(BindingContext is PalletesViewModel viewModel)
        {
            await viewModel.LoadPalletesAsyncOrderedDescByCreatedAtUtc();
        }
    }

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {

    }
}