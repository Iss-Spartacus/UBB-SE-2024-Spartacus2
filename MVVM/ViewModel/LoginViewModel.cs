using business_social_media.Services;
using bussiness_social_media.Core;
using bussiness_social_media.Services;

namespace bussiness_social_media.MVVM.ViewModel
{
    internal class LoginViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        private AuthenticationService authenticationService;

        private string _username;
        private string _password;
        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

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

        public RelayCommand LogInCommand { get; set; }

        private void LogIn()
        {
            if(authenticationService.AuthenticateUser(Username, Password)) {
                ErrorMessage = "";

                _navigation.NavigateTo<HomeViewModel>();
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }

        public RelayCommand NavigateToRegisterViewCommand { get; set; }

        public LoginViewModel(INavigationService navigationService, AuthenticationService authentication)
        {
            NavigationService = navigationService;
            authenticationService = authentication;
            LogInCommand = new RelayCommand(o => { LogIn();  }, o => true);
            NavigateToRegisterViewCommand = new RelayCommand(o => { NavigationService.NavigateTo<RegisterViewModel>(); }, o => true);


        }

    }
}
