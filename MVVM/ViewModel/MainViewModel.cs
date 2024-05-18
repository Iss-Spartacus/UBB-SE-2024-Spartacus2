using business_social_media.Services;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bussiness_social_media.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        private AuthenticationService _authenticationService;
        public Visibility IsLoggedInMenuItemsVisibility { get; set; }
        public Visibility IsNotLoggedInMenuItemsVisibility { get; set; }

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public AuthenticationService AuthenticationService
        {
            get => _authenticationService;
            set
            {
                _authenticationService = value;
                OnPropertyChanged();
            }
        }




        public RelayCommand NavigateToHomeCommand { get; set; }
        public RelayCommand NavigateToCreateNewBusinessViewCommand{ get; set; }
        public RelayCommand NavigateToUserManagedBusinessesViewCommand { get; set; }
        public MainViewModel(INavigationService navigationService, AuthenticationService authenticationService)
        {
            Navigation = navigationService;
            AuthenticationService = authenticationService;
            NavigateToHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
            NavigateToCreateNewBusinessViewCommand = new RelayCommand(o => { Navigation.NavigateTo<CreateNewBusinessViewModel>(); }, o => true);
            NavigateToUserManagedBusinessesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<UserManagedBusinessPagesViewModel>();
            }, o => true);
            Navigation.NavigateTo<LoginViewModel>();
            IsLoggedInMenuItemsVisibility = Visibility.Collapsed;
            IsNotLoggedInMenuItemsVisibility = Visibility.Visible;
            AuthenticationService.LoginStatusChanged += OnLoginStatusChanged;
        }

        private void OnLoginStatusChanged(object sender, EventArgs e)
        {
            UpdateButtonVisibility();
        }

        public void UpdateButtonVisibility()
        {
            IsLoggedInMenuItemsVisibility = AuthenticationService.GetIsLoggedIn() ? Visibility.Visible : Visibility.Collapsed;
            IsNotLoggedInMenuItemsVisibility = AuthenticationService.GetIsLoggedIn() ? Visibility.Collapsed : Visibility.Visible;
        }

    }
}
