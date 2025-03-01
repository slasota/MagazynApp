using MagazynApp.Models;
using MagazynApp.ViewModels;
using Microsoft.Maui.Platform;

namespace MagazynApp.Views;

public partial class AddProductPage : ContentPage
{

	public AddProductPage(AddProductViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}