using bussiness_social_media.Core;
using bussiness_social_media.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using business_social_media.Services;

namespace bussiness_social_media.MVVM.ViewModel
{
    class UserManagedBusinessPagesViewModel : Core.ViewModel
    {
        private readonly IBusinessService _businessService;
        private INavigationService _navigation;
        private readonly AuthenticationService _authenticationService;
        private string _userId;
        private string _noBusinessMessage;

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToBusinessProfileViewCommand { get; set; }
        public ObservableCollection<Business> Businesses
        {
            get
            {
                UserId = _authenticationService.GetIsLoggedIn() ? _authenticationService.CurrentUser.Username : string.Empty;
                NoBusinessMessage = UserId == string.Empty ? "" : "You are not managing any businesses";
                return new ObservableCollection<Business>(_businessService.GetBusinessesManagedBy(UserId));
            }
        }

        public string NoBusinessMessage
        {
            get
            {
                if (_authenticationService.GetIsLoggedIn())
                {
                    return "";
                }
                else
                {
                    return "Ups.. You have no businesses.";
                }
            }
            set
            {
                _noBusinessMessage = value;
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

        public UserManagedBusinessPagesViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            NavigationService = navigationService;
            _businessService = businessService;
            _authenticationService = authenticationService;

            UserId = _authenticationService.GetIsLoggedIn() ? _authenticationService.CurrentUser.Username : string.Empty;

            NavigateToBusinessProfileViewCommand = new RelayCommand(o =>
            {
                if (o is Business business)
                {
                    NavigationService.BusinessId = business.Id;
                    NavigationService.NavigateTo<BusinessProfileViewModel>();
                }
            }, o => true);
            _businessService = businessService;
            _businessService = businessService;
        }

       
    }
}
