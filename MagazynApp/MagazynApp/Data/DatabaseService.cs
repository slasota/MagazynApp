using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagazynApp.Models;
using SQLite;

namespace MagazynApp.Data
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath); 
        }

        public async Task InitalizeAsync()
        {
            await _database.CreateTableAsync<Produkt>();
            await _database.CreateTableAsync<Pallete>();
            await _database.CreateTableAsync<PaletaProdukt>();
            await _database.CreateIndexAsync<Produkt>(p => p.Id);
        }

        // Dodanie nowego produktu
        public async Task<int> DodajProduktAsync(Produkt produkt)
        {
            return await _database.InsertAsync(produkt);
        }

        // Pobranie produktów
        public async Task<List<Produkt>> PobierzProduktyAsync()
        {
            return await _database.Table<Produkt>().ToListAsync();
        }

        //public async Task<List<Produkt>> PobierzProduktyAsync(int offset, int limit)
        //{
        //    return await _database.Table<Produkt>()
        //        .OrderBy(p => p.Id)
        //        .Skip(offset)
        //        .Take(limit)
        //        .ToListAsync();
        //}

        public async Task AktualizujProduktAsync(Produkt produkt)
        {
            if (produkt == null) throw new ArgumentNullException(nameof(produkt));

            await _database.UpdateAsync(produkt);
        }

        public async Task<Produkt> PobierzProduktAsync(int id)
        {
            return await _database.GetAsync<Produkt>(id);
        }

        public async Task<int> UsunProduktAsync(Produkt produkt)
        {
            return await _database.DeleteAsync(produkt);
        }

        public async Task<int> UsunWysztkieProdukty()
        {
            return await _database.DeleteAllAsync<Produkt>();
        }

        //CRUD PALLET

        //Dodawanie palety
        public async Task<bool> AddPalleteAsync(Pallete pallete)
        {
            try
            {
                int rowsAffected = await _database.InsertAsync(pallete);
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding pallete: {e.Message}");
                return false;
            }
        }
        //Pobieranie palety
        public async Task<Pallete> GetPalleteAsync(int id)
        {
            try
            {
                return await _database.GetAsync<Pallete>(id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting pallete: {e.Message}");
                return null;
            }

        }
        //Pobieranie listy palet posortowanych po dacie
        public async Task<List<Pallete>> GetPalletesAsync()
        {
            try { 
                return await _database.Table<Pallete>().ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting palletes: {e.Message}");
                return new List<Pallete>();
            }   
        }
        //Usuwanie palety
        public async Task<bool> DeletePalleteAsync(Pallete pallet)
        {
            try
            {
                int rowAffected = await _database.DeleteAsync(pallet);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: DeletePallete: {ex}");
                return false;
            }
        }
        //Edytowanie palety
        public async Task<bool> EditPalleteAsync(Pallete pallet)
        {
            if (pallet == null) throw new ArgumentNullException();

            try
            {
                int rowsAffected = await _database.UpdateAsync(pallet);
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error editing pallete: {e.Message}");
                return false;
            }
        }
    }
}
