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
    [QueryProperty(nameof(PalleteId), "palleteId")]
    public partial class AddEditPalleteViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        private static DateTime palleteCreatedAtUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        [ObservableProperty]
        private int _palleteId;
        [ObservableProperty]
        private string _palleteName;
        [ObservableProperty]
        private DateTime _palleteCreatedAtLocal = palleteCreatedAtUtc.ToLocalTime();
        
        
        [ObservableProperty]
        private string _addEditButtonTitle = "Dodaj paletę";

        public AddEditPalleteViewModel(DatabaseService database)
        {
            _databaseService = database;
            
        }
        partial void OnPalleteIdChanged(int value)
        {
            Console.WriteLine($"DEBUG: Otrzymano palleteId: {value}");
            if (value > 0)
            {
                AddEditButtonTitle = "Zapisz zmiany";
                Task.Run(async () => await GetPallete(value));
            }
        }

        [RelayCommand]
        public async Task AddEditPallete()
        {
            if (String.IsNullOrWhiteSpace(PalleteName))
            {
                await Shell.Current.DisplayAlert("Błąd", "Podaj nazwę palety!", "Ok");
                return;
            }
            if (PalleteId > 0)
            {
                var pallete = await _databaseService.GetPalleteAsync(PalleteId);
                if (pallete != null)
                {
                    pallete.PalleteName = PalleteName;
                    //pallete.CreatedAtUtc = PalleteCreatedAtUtc;
                    bool success = await _databaseService.EditPalleteAsync(pallete);

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
                var pallete = new Pallete
                {
                    PalleteName = PalleteName,
                    CreatedAtUtc = DateTime.UtcNow
                };
                
                bool success = await _databaseService.AddPalleteAsync(pallete);

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

        private async Task GetPallete(int palleteId)
        {
            try
            {
                var pallete = await _databaseService.GetPalleteAsync(palleteId);
                if (pallete != null)
                {
                    PalleteName = pallete.PalleteName;
                    PalleteCreatedAtLocal = pallete.CreatedAtUtc.ToLocalTime();
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
