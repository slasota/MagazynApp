using MagazynApp.Models;

namespace MagazynApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Dodajemy testowy produkt
        await App.Database.DodajProduktAsync(new Product { Name = "Brit Care Adult Large Lamb 12kg", BarCode = "123456789" });

        // Pobieramy produkty i wypisujemy w konsoli
        var produkty = await App.Database.PobierzProduktyAsync();
        foreach (var produkt in produkty)
        {
            Console.WriteLine($"Product: {produkt.Name}, Kod kreskowy: {produkt.BarCode}");
        }

    }
}
