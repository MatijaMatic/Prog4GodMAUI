using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prog4GodMAUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        [ObservableProperty]
        string title;

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        [RelayCommand]
        private async Task GoToRoute(string pageName)
        {
            await Shell.Current.GoToAsync($"//{pageName}");
        }
    }
}
