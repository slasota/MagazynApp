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

            builder.Services.AddTransient<AddProductPage>();
            builder.Services.AddTransient<AddProductViewModel>();
            builder.Services.AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<ProductsPage>();
            builder.Services.AddTransient<PalletsViewModel>();
            builder.Services.AddTransient<PalletPage>();  
            builder.Services.AddTransient<AddEditPalletPage>();
            builder.Services.AddTransient<AddEditPalletViewModel>();

            return builder.Build();
        }
    }
}
