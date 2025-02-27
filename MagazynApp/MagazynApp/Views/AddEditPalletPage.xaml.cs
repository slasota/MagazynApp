using MagazynApp.ViewModels;

namespace MagazynApp.Views;

public partial class AddEditPalletPage : ContentPage
{
	public AddEditPalletPage(AddEditPalletViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
	
}