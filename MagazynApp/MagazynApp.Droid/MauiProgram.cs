using MagazynApp.Data;
using MagazynApp.ViewModels;
using MagazynApp.Views;

namespace MagazynApp.Droid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseSharedMauiApp();

            // ścieżka do bazy danych
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "magazyn.db");

            // Rejestracja DatabaseService w Depedence Injection DI
            builder.Services.AddSingleton<DatabaseService>(s => new DatabaseService(dbPath));

            builder.Services.AddTransient<DodajProduktPage>();
            builder.Services.AddTransient<DodajProduktViewModel>();
            builder.Services.AddTransient<ProduktyViewModel>();
            builder.Services.AddTransient<ProduktyPage>();
            builder.Services.AddTransient<PalletesViewModel>();
            builder.Services.AddTransient<PalletePage>();  
            builder.Services.AddTransient<AddEditPalletePage>();
            builder.Services.AddTransient<AddEditPalleteViewModel>();

            return builder.Build();
        }
    }
}
