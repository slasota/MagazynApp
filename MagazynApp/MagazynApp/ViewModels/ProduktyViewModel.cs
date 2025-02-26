using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagazynApp.Data;
using MagazynApp.Models;

namespace MagazynApp.ViewModels
{
    public partial class ProduktyViewModel : ObservableObject
    {

        private readonly DatabaseService _databaseService;
        [ObservableProperty]
        private ObservableCollection<Produkt> _produkty = new();

        //public bool CzyWczytuje { get; private set; }

        public ProduktyViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            MainThread.BeginInvokeOnMainThread(async () => await WczytajProduktyAsync());

        }

        [RelayCommand]
        public async Task Odswiez()
        {
            await WczytajProduktyAsync();
        }
        public async Task WczytajProduktyAsync()
        {

            try
            {
                var listaProduktow = await Task.Run(async () =>
                {
                    return await _databaseService.PobierzProduktyAsync();
                });

                Produkty.Clear();
                foreach (var item in listaProduktow)
                {
                        Produkty.Add(item);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd pobierania danych: {ex.Message}");
            }
        }

        //[RelayCommand]
        //public async Task WczytajKolejneProdukty()
        //{
        //    if (CzyWczytuje) return; // Unikamy podwójnego wczytywania
        //    CzyWczytuje = true;

        //    var noweProdukty = await _databaseService.PobierzProduktyAsync(Produkty.Count, 20);

        //    //MainThread.BeginInvokeOnMainThread(() =>
        //    //{
        //    foreach (var produkt in noweProdukty)
        //    { 
        //        Produkty.Add(produkt);
        //    }
        //    //});

        //    CzyWczytuje = false;
        //}

        [RelayCommand]
        public async Task DodajProduktPage()
        {
            await Shell.Current.GoToAsync(nameof(DodajProduktPage));
        }

        [RelayCommand]
        public async Task EdytujProdukt(Produkt produkt)
        {
            Console.WriteLine($"DEBUG: Próba edycji produktu: {produkt.Id}");
            await Shell.Current.GoToAsync($"DodajProduktPage?produktId={produkt.Id}");
        }
        [RelayCommand]
        public async Task UsunProdukt(Produkt produkt)
        {
            if (produkt == null) return;

            bool potwierdzenie = await Shell.Current.DisplayAlert(
                "Usuń produkt",
                $"Czy na pewno chcesz usunąć {produkt.Nazwa}?",
                "Tak",
                "Nie"
                );

            if (!potwierdzenie) return;

            await _databaseService.UsunProduktAsync(produkt);

            Produkty.Remove(produkt);
        }

        [RelayCommand]
        public async Task DeleteAllProducts()
        {
            bool potwierdzenie = await Shell.Current.DisplayAlert(
                "Usuń wszystkie produkty",
                $"Czy na pewno chcesz usunąć wszystkie produkty?",
                "Tak",
                "Nie"
                );
            if (!potwierdzenie) return;
            await _databaseService.UsunWysztkieProdukty();
            Produkty.Clear();
        }
    }
}
