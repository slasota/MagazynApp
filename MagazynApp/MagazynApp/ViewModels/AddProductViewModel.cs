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
    [QueryProperty(nameof(ProductId), "productId")]
    public partial class AddProductViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string _productName;
        [ObservableProperty]
        private string _barCode;

        [ObservableProperty]
        private int _productId;

        [ObservableProperty]
        private string _buttonLabel;

        public AddProductViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            ButtonLabel = "Dodaj product";
        }

        partial void OnProductIdChanged(int value)
        {
            Console.WriteLine($"DEBUG: Otrzymano productId: {value}");
            if (value > 0)
            {
                ButtonLabel = "Zapisz zmiany";
                SetProperty(ref _productId, value);
                Task.Run(async () => await GetProduct(value)).ConfigureAwait(false);
            }
        }

        [RelayCommand]
        public async Task AddOrUpdateProduct()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(ProductName))
                {
                    await Shell.Current.DisplayAlert("Błąd", "Podaj nazwę produktu!", "Ok");
                    return;
                }

                if (ProductId > 0)
                {
                    var produkt = await _databaseService.PobierzProduktAsync(ProductId);
                    if (produkt != null)
                    {
                        produkt.Name = ProductName;
                        produkt.BarCode = BarCode;
                        await _databaseService.AktualizujProduktAsync(produkt);
                    }
                    ButtonLabel = "Dodaj product";
                }
                else
                {
                    var nowyProdukt = new Product
                    {
                        Name = ProductName,
                        BarCode = BarCode
                    };

                    await _databaseService.DodajProduktAsync(nowyProdukt);
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas dodawania produktu.", "Ok");
            }
        }

        private async Task GetProduct(int productId)
        {
            try
            {
                var product = await _databaseService.PobierzProduktAsync(productId);
                if (product != null)
                {
                    ProductName = product.Name;
                    BarCode = product.BarCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas wczytywania produktu.", "Ok");
            }
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
