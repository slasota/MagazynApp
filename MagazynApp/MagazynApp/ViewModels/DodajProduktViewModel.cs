using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagazynApp.Data;
using MagazynApp.Models;

namespace MagazynApp.ViewModels
{
    [QueryProperty(nameof(ProduktId), "produktId")]
    public partial class DodajProduktViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string _nazwaProduktu;
        [ObservableProperty]
        private string _kodKreskowy;

        [ObservableProperty]
        private int _produktId;

        [ObservableProperty]
        private string _tytulPrzycisku;

        public DodajProduktViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            TytulPrzycisku = "Dodaj produkt";
        }

        partial void OnProduktIdChanged(int value)
        {
            Console.WriteLine($"DEBUG: Otrzymano produktId: {value}");
            if (value > 0)
            {
                TytulPrzycisku = "Zapisz zmiany";
                SetProperty(ref _produktId, value);
                Task.Run(async () => await WczytajProdukt(value)).ConfigureAwait(false);
            }
        }

        [RelayCommand]
        public async Task DodajProdukt()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(NazwaProduktu))
                {
                    await Application.Current.MainPage.DisplayAlert("Błąd", "Podaj nazwę produktu!", "Ok");
                    return;
                }

                if (ProduktId > 0)
                {
                    var produkt = await _databaseService.PobierzProduktAsync(ProduktId);
                    if (produkt != null)
                    {
                        produkt.Nazwa = NazwaProduktu;
                        produkt.KodKreskowy = KodKreskowy;
                        await _databaseService.AktualizujProduktAsync(produkt);
                    }
                    TytulPrzycisku = "Dodaj produkt";
                }
                else
                {
                    var nowyProdukt = new Produkt
                    {
                        Nazwa = NazwaProduktu,
                        KodKreskowy = KodKreskowy
                    };

                    await _databaseService.DodajProduktAsync(nowyProdukt);
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Błąd", "Wystąpił błąd podczas dodawania produktu.", "Ok");
            }
        }

        private async Task WczytajProdukt(int produktId)
        {
            try
            {
                var produkt = await _databaseService.PobierzProduktAsync(produktId);
                if (produkt != null)
                {
                    NazwaProduktu = produkt.Nazwa;
                    KodKreskowy = produkt.KodKreskowy;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Błąd", "Wystąpił błąd podczas wczytywania produktu.", "Ok");
            }
        }

        [RelayCommand]
        public async Task Anuluj()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
