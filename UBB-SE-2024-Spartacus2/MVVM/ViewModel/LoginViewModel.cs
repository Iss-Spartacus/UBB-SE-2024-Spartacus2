using Bussiness_social_media.Core;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.MVVM.ViewModel
{
    public class LoginViewModel : Core.ViewModel
    {
        private INavigationService navigation;
        private AuthenticationService authenticationService;
        private string username;
        private string password;
        private string errorMessage;

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

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

        public RelayCommand LogInCommand { get; set; }
        public RelayCommand NavigateToRegisterViewCommand { get; set; }

        public LoginViewModel(INavigationService navigationService, AuthenticationService authentication)
        {
            NavigationService = navigationService;
            authenticationService = authentication;
            LogInCommand = new RelayCommand(o => { LogIn(); }, o => true);
            NavigateToRegisterViewCommand = new RelayCommand(o => { NavigationService.NavigateTo<RegisterViewModel>(); }, o => true);
        }

        private void LogIn()
        {
            if (authenticationService.AuthenticateUser(Username, Password))
            {
                ErrorMessage = string.Empty;
                navigation.NavigateTo<HomeViewModel>();
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }
    }
}
