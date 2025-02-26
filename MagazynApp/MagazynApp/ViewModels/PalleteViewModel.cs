using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagazynApp.Data;
using MagazynApp.Models;
using MagazynApp.Views;

namespace MagazynApp.ViewModels
{
    public partial class PalleteViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Pallete> _palletes = new();

        public PalleteViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            MainThread.BeginInvokeOnMainThread(async () => await LoadPalletesAsync());
            
        }

        [RelayCommand]
        public async Task LoadPalletesAsync()
        {
            var listPallet = await _databaseService.GetPalletsAsync();

            Palletes.Clear();

            foreach (var pallete in listPallet)
            {
                Palletes.Add(pallete);
            }
        }

        [RelayCommand]
        public async Task AddPallete()
        {
            await Shell.Current.GoToAsync(nameof(AddEditPalletePage));

        }

        [RelayCommand]
        public async Task EditPallete(Pallete pallete)
        {
            await Shell.Current.GoToAsync($"AddEditPalletePage?palleteId={pallete.Id}");
        }
    }
}
