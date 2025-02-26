using MagazynApp.Models;
using MagazynApp.ViewModels;
using Microsoft.Maui.Platform;

namespace MagazynApp.Views;

public partial class DodajProduktPage : ContentPage
{

	public DodajProduktPage(DodajProduktViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}