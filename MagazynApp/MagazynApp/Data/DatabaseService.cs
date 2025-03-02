﻿using System;
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
            await _database.CreateTableAsync<Product>();
            await _database.CreateTableAsync<Pallet>();
            await _database.CreateTableAsync<PalletProduct>();
            await _database.CreateIndexAsync<Product>(p => p.Id);
        }

        // Dodanie nowego produktu
        public async Task<int> DodajProduktAsync(Product produkt)
        {
            return await _database.InsertAsync(produkt);
        }

        // Pobranie produktów
        public async Task<List<Product>> PobierzProduktyAsync()
        {
            return await _database.Table<Product>().ToListAsync();
        }

        //public async Task<List<Product>> PobierzProduktyAsync(int offset, int limit)
        //{
        //    return await _database.Table<Product>()
        //        .OrderBy(p => p.Id)
        //        .Skip(offset)
        //        .Take(limit)
        //        .ToListAsync();
        //}

        public async Task AktualizujProduktAsync(Product produkt)
        {
            if (produkt == null) throw new ArgumentNullException(nameof(produkt));

            await _database.UpdateAsync(produkt);
        }

        public async Task<Product> PobierzProduktAsync(int id)
        {
            return await _database.GetAsync<Product>(id);
        }

        public async Task<int> UsunProduktAsync(Product produkt)
        {
            return await _database.DeleteAsync(produkt);
        }

        public async Task<int> UsunWysztkieProdukty()
        {
            return await _database.DeleteAllAsync<Product>();
        }

        //CRUD PALLET

        //Dodawanie palety
        public async Task<bool> AddPalletAsync(Pallet pallet)
        {
            try
            {
                int rowsAffected = await _database.InsertAsync(pallet);
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding pallet: {e.Message}");
                return false;
            }
        }
        //Pobieranie palety
        public async Task<Pallet> GetPalletAsync(int id)
        {
            try
            {
                return await _database.GetAsync<Pallet>(id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting pallet: {e.Message}");
                return null;
            }

        }
        //Pobieranie listy palet posortowanych po dacie
        public async Task<List<Pallet>> GetPalletsAsync()
        {
            try { 
                return await _database.Table<Pallet>().ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting palletes: {e.Message}");
                return new List<Pallet>();
            }   
        }
        //Usuwanie palety
        public async Task<bool> DeletePalletAsync(Pallet pallet)
        {
            try
            {
                int rowAffected = await _database.DeleteAsync(pallet);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: DeletePallet: {ex}");
                return false;
            }
        }
        //Edytowanie palety
        public async Task<bool> EditPalletAsync(Pallet pallet)
        {
            if (pallet == null) throw new ArgumentNullException();

            try
            {
                int rowsAffected = await _database.UpdateAsync(pallet);
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error editing pallet: {e.Message}");
                return false;
            }
        }
    }
}
