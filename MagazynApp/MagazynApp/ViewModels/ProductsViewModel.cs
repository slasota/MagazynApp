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
    public partial class ProductsViewModel : ObservableObject
    {

        private readonly DatabaseService _databaseService;
        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        //public bool CzyWczytuje { get; private set; }

        public ProductsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            MainThread.BeginInvokeOnMainThread(async () => await GetProductsAsync());

        }

        [RelayCommand]
        public async Task Refresh()
        {
            await GetProductsAsync();
        }
        public async Task GetProductsAsync()
        {

            try
            {
                var listaProduktow = await Task.Run(async () =>
                {
                    return await _databaseService.PobierzProduktyAsync();
                });

                Products.Clear();
                foreach (var item in listaProduktow)
                {
                        Products.Add(item);
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
        //    foreach (var product in noweProdukty)
        //    { 
        //        Produkty.Add(product);
        //    }
        //    //});

        //    CzyWczytuje = false;
        //}

        [RelayCommand]
        public async Task AddProductPage()
        {
            await Shell.Current.GoToAsync(nameof(AddProductPage));
        }

        [RelayCommand]
        public async Task EditProduct(Product product)
        {
            Console.WriteLine($"DEBUG: Próba edycji produktu: {product.Id}");
            await Shell.Current.GoToAsync($"AddProductPage?productId={product.Id}");
        }
        [RelayCommand]
        public async Task DeleteProduct(Product product)
        {
            if (product == null) return;

            bool isConfirmed = await Shell.Current.DisplayAlert(
                "Usuń product",
                $"Czy na pewno chcesz usunąć {product.Name}?",
                "Tak",
                "Nie"
                );

            if (!isConfirmed) return;

            await _databaseService.UsunProduktAsync(product);

            Products.Remove(product);
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
            Products.Clear();
        }
    }
}
