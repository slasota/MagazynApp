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
    public partial class PalletesViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Pallet> _palletes = new();

        public PalletesViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            MainThread.BeginInvokeOnMainThread(async () => await LoadPalletesAsyncOrderedDescByCreatedAtUtc());
            
        }

        public async Task LoadPalletesAsyncOrderedDescByCreatedAtUtc()
        {
            try
            {
                var listPallet = await _databaseService.GetPalletesAsync();

                Palletes.Clear();

                foreach (var pallet in listPallet)
                {
                    pallet.CreatedAtUtc = pallet.CreatedAtUtc.ToLocalTime();
                    Palletes.Add(pallet);
                }
                Palletes.OrderByDescending(p => p.CreatedAtUtc).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while loading palletes: {ex.Message}");
                await Shell.Current.DisplayAlert("Błąd", "Error while loading palletes", "Ok");
            }
        }

        [RelayCommand]
        public async Task AddPallet()
        {
            await Shell.Current.GoToAsync(nameof(AddEditPalletPage));

        }

        [RelayCommand]
        public async Task EditPallet(Pallet pallet)
        {
            await Shell.Current.GoToAsync($"AddEditPalletPage?palletId={pallet.Id}");
        }

        [RelayCommand]
        public async Task DeletePallet(Pallet pallet)
        {

            if (pallet == null) return;

            bool confirmation = await Shell.Current.DisplayAlert(
                "Usuń paletę",
                $"Czy na pewno chcesz usunąć {pallet.PalletName}?",
                "Tak",
                "Nie"
                );

            if (!confirmation) return;  

            

            bool success = await _databaseService.DeletePalletAsync(pallet);
            if (!success) await Shell.Current.DisplayAlert("Błąd", "Błąd podczas usuwania palety", "Ok");
            else
            { Palletes.Remove(pallet); }
            
        }
    }
}
