using MagazynApp.Models;
using MagazynApp.ViewModels;

namespace MagazynApp.Views;

public partial class ProduktyPage : ContentPage
{
	public ProduktyPage(ProduktyViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProduktyViewModel viewModel)
        {
            await viewModel.WczytajProduktyAsync(); // Wczytaj produkty przy wejœciu na stronê
        }
    }
}