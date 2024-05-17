using business_social_media.Services;
using bussiness_social_media.Core;
using bussiness_social_media.MVVM.Model.Repository;
using bussiness_social_media.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace bussiness_social_media.MVVM.ViewModel
{
    public class RegisterViewModel : Core.ViewModel
    {
        private IUserRepository _userRepository;
        private INavigationService _navigation;
        private AuthenticationService authenticationService;

        private string _username;
        private string _password;
        public INavigationService NavigationService
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
       
        public RelayCommand RegisterCommand { get; set; }
        private void Register()
        {

            string hashedPassword = _userRepository.GetMd5Hash(Password);
            if (authenticationService.AuthenticateUser(Username, hashedPassword))
            {
                MessageBox.Show("User already registered!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Account newAccount = new Account(Username, hashedPassword);
                _userRepository.AddAccount(newAccount);
                NavigationService.NavigateTo<HomeViewModel>();

            }

        }
        public RegisterViewModel(INavigationService navigationService, AuthenticationService authentication,IUserRepository userRepository)
        {
            NavigationService = navigationService;
            authenticationService = authentication;
            _userRepository = userRepository;
            RegisterCommand = new RelayCommand(o => { Register(); }, o => true);

        }
    }
}
