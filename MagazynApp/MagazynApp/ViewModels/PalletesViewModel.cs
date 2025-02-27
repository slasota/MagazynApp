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
        private ObservableCollection<Pallete> _palletes = new();

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

                foreach (var pallete in listPallet)
                {
                    pallete.CreatedAtUtc = pallete.CreatedAtUtc.ToLocalTime();
                    Palletes.Add(pallete);
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
        public async Task AddPallete()
        {
            await Shell.Current.GoToAsync(nameof(AddEditPalletePage));

        }

        [RelayCommand]
        public async Task EditPallete(Pallete pallete)
        {
            await Shell.Current.GoToAsync($"AddEditPalletePage?palleteId={pallete.Id}");
        }

        [RelayCommand]
        public async Task DeletePallete(Pallete pallete)
        {

            if (pallete == null) return;

            bool confirmation = await Shell.Current.DisplayAlert(
                "Usuń paletę",
                $"Czy na pewno chcesz usunąć {pallete.PalleteName}?",
                "Tak",
                "Nie"
                );

            if (!confirmation) return;  

            

            bool success = await _databaseService.DeletePalleteAsync(pallete);
            if (!success) await Shell.Current.DisplayAlert("Błąd", "Błąd podczas usuwania palety", "Ok");
            else
            { Palletes.Remove(pallete); }
            
        }
    }
}
