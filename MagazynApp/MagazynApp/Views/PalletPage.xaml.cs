using System.Threading.Tasks;
using MagazynApp.ViewModels;

namespace MagazynApp.Views;

public partial class PalletPage : ContentPage
{
	public PalletPage(PalletsViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if(BindingContext is PalletsViewModel viewModel)
        {
            await viewModel.LoadPalletsAsync();
        }
    }

}