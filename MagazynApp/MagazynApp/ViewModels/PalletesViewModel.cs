﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagazynApp.Data;
using MagazynApp.Models;
using MagazynApp.Views;

namespace MagazynApp.ViewModels
{
    public partial class PalletesViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        private List<Pallet> _listPallet = new();

        private CancellationTokenSource _debounceCts;

        private bool _isAsceding = false;

        [ObservableProperty]
        private string _searchText;
        [ObservableProperty]
        private bool _searchBarVisible = false;

        [ObservableProperty]
        private ObservableCollection<Pallet> _palletes = new();

        public PalletesViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            MainThread.BeginInvokeOnMainThread(async () => await LoadPalletesAsync());
        }


        // Metoda Debounce (uruchamia się 500ms po wpisaniu ostatniej litery)
        partial void OnSearchTextChanged(string value)
        {
            _debounceCts?.Cancel();
            _debounceCts = new CancellationTokenSource();

            var token = _debounceCts.Token;

            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(250, token);
                    if (!token.IsCancellationRequested)
                    {
                        MainThread.BeginInvokeOnMainThread(async () => await FilterPalletes());
                    }
                }
                catch (TaskCanceledException) { }
            });
        }

        private async Task FilterPalletes()
        {

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var filtered = Palletes.Where(p => p.PalletName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                Palletes = new ObservableCollection<Pallet>(filtered);
            }
            else
            {
                Palletes = new ObservableCollection<Pallet>(_listPallet);   
            }

        }

        [RelayCommand]
        public void ToggleSortOrder()
        {
            _isAsceding = !_isAsceding;
            MainThread.BeginInvokeOnMainThread(async () => {
                await LoadPalletesAsync();
                await FilterPalletes();
                });
            
        }

        [RelayCommand]
        public void ToggleSearchBarVisibility()
        {
            SearchBarVisible = !SearchBarVisible;
        }

        public async Task LoadPalletesAsync()
        {
            try
            {
                _listPallet = await _databaseService.GetPalletesAsync();

                if (_isAsceding)
                    _listPallet = _listPallet.OrderBy(p => p.CreatedAtUtc).ToList();
                else
                    _listPallet = _listPallet.OrderByDescending(p => p.CreatedAtUtc).ToList();
                Palletes.Clear();

                foreach (var pallet in _listPallet)
                {
                    pallet.CreatedAtUtc = pallet.CreatedAtUtc.ToLocalTime();
                    Palletes.Add(pallet);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while loading palletes: {ex.Message}");
                await Shell.Current.DisplayAlert("Błąd", "Error while loading palletes", "Ok");
            }
        }

        [RelayCommand]
        public async Task AddPallet()
        {
            await Shell.Current.GoToAsync(nameof(AddEditPalletPage));

        }

        [RelayCommand]
        public async Task EditPallet(Pallet pallet)
        {
            await Shell.Current.GoToAsync($"AddEditPalletPage?palletId={pallet.Id}");
        }

        [RelayCommand]
        public async Task DeletePallet(Pallet pallet)
        {

            if (pallet == null) return;

            bool confirmation = await Shell.Current.DisplayAlert(
                "Usuń paletę",
                $"Czy na pewno chcesz usunąć {pallet.PalletName}?",
                "Tak",
                "Nie"
                );

            if (!confirmation) return;



            bool success = await _databaseService.DeletePalletAsync(pallet);
            if (!success) await Shell.Current.DisplayAlert("Błąd", "Błąd podczas usuwania palety", "Ok");
            else
            { Palletes.Remove(pallet); }

        }

        //    [RelayCommand]
        //    public async Task OnSortClicked(object sender, EventArgs e)
        //    {
        //        ToggleSortOrder();
        //    }
        //}
    }
}
