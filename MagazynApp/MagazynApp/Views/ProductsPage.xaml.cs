using MagazynApp.Models;
using MagazynApp.ViewModels;

namespace MagazynApp.Views;

public partial class ProductsPage : ContentPage
{
	public ProductsPage(ProductsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProductsViewModel viewModel)
        {
            await viewModel.GetProductsAsync(); // Wczytaj produkty przy wejœciu na stronê
        }
    }
}