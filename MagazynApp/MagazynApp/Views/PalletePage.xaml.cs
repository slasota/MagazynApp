using System.Threading.Tasks;
using MagazynApp.ViewModels;

namespace MagazynApp.Views;

public partial class PalletePage : ContentPage
{
	public PalletePage(PalletesViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if(BindingContext is PalletesViewModel viewModel)
        {
            await viewModel.LoadPalletesAsync();
        }
    }
}