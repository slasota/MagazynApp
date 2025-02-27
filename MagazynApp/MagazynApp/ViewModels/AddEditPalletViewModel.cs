using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagazynApp.Data;
using MagazynApp.Models;

namespace MagazynApp.ViewModels
{
    [QueryProperty(nameof(PalletId), "palletId")]
    public partial class AddEditPalletViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        private static DateTime palletCreatedAtUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        [ObservableProperty]
        private int _palletId;
        [ObservableProperty]
        private string _palletName;
        [ObservableProperty]
        private DateTime _palletCreatedAtLocal = palletCreatedAtUtc.ToLocalTime();
        
        
        [ObservableProperty]
        private string _addEditButtonTitle = "Dodaj paletę";

        public AddEditPalletViewModel(DatabaseService database)
        {
            _databaseService = database;
            
        }
        partial void OnPalletIdChanged(int value)
        {
            Console.WriteLine($"DEBUG: Otrzymano palletId: {value}");
            if (value > 0)
            {
                AddEditButtonTitle = "Zapisz zmiany";
                Task.Run(async () => await GetPallet(value));
            }
        }

        [RelayCommand]
        public async Task AddEditPallet()
        {
            if (String.IsNullOrWhiteSpace(PalletName))
            {
                await Shell.Current.DisplayAlert("Błąd", "Podaj nazwę palety!", "Ok");
                return;
            }
            if (PalletId > 0)
            {
                var pallet = await _databaseService.GetPalletAsync(PalletId);
                if (pallet != null)
                {
                    pallet.PalletName = PalletName;
                    //pallet.CreatedAtUtc = PalletCreatedAtUtc;
                    bool success = await _databaseService.EditPalletAsync(pallet);

                    if(success)
                    {
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Błąd", "Błąd podczas edytowania palety", "Ok");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd", "Błąd podczas pobrania palety do edycji", "Ok");
                }
            }
            else
            {
                var pallet = new Pallet
                {
                    PalletName = PalletName,
                    CreatedAtUtc = DateTime.UtcNow
                };
                
                bool success = await _databaseService.AddPalletAsync(pallet);

                if(success)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd", "Błąd podczas dodawania palety", "Ok");
                }


            }
        }

        private async Task GetPallet(int palletId)
        {
            try
            {
                var pallet = await _databaseService.GetPalletAsync(palletId);
                if (pallet != null)
                {
                    PalletName = pallet.PalletName;
                    PalletCreatedAtLocal = pallet.CreatedAtUtc.ToLocalTime();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: Błąd podczas pobierania palety: {ex.Message}");

            }
        }
        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
