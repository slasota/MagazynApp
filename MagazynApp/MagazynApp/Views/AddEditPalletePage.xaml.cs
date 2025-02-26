using MagazynApp.ViewModels;

namespace MagazynApp.Views;

public partial class AddEditPalletePage : ContentPage
{
	public AddEditPalletePage(AddEditPalleteViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
	
}