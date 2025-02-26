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
        await App.Database.DodajProduktAsync(new Produkt { Nazwa = "Brit Care Adult Large Lamb 12kg", KodKreskowy = "123456789" });

        // Pobieramy produkty i wypisujemy w konsoli
        var produkty = await App.Database.PobierzProduktyAsync();
        foreach (var produkt in produkty)
        {
            Console.WriteLine($"Produkt: {produkt.Nazwa}, Kod kreskowy: {produkt.KodKreskowy}");
        }

    }
}
