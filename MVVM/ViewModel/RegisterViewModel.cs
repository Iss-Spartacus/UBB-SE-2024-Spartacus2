using System.Windows;
using Bussiness_social_media.Core;
using Bussiness_social_media.MVVM.Model.Repository;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.MVVM.ViewModel
{
    public class RegisterViewModel : Core.ViewModel
    {
        private IUserRepository userRepository;
        private INavigationService navigation;
        private AuthenticationService authenticationService;
        private string username;
        private string password;

        public INavigationService NavigationService
        {
            get => navigation;
            set
            {
                navigation = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RegisterCommand { get; set; }

        public RegisterViewModel(INavigationService navigationService, AuthenticationService authentication, IUserRepository userRepository)
        {
            NavigationService = navigationService;
            authenticationService = authentication;
            this.userRepository = userRepository;
            RegisterCommand = new RelayCommand(o => { Register(); }, o => true);
        }

        private void Register()
        {
            string hashedPassword = userRepository.GetMd5Hash(Password);
            if (authenticationService.AuthenticateUser(Username, hashedPassword))
            {
                MessageBox.Show("User already registered!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Account newAccount = new Account(Username, hashedPassword);
                userRepository.AddAccount(newAccount);
                NavigationService.NavigateTo<HomeViewModel>();
            }
        }
    }
}
