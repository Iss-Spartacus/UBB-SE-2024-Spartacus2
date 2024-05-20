using System.Collections.ObjectModel;
using System.Windows.Navigation;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.MVVM.ViewModel
{
    public class UserManagedBusinessPagesViewModel : Core.ViewModel
    {
        private readonly IBusinessService businessService;
        private INavigationService navigation;
        private readonly AuthenticationService authenticationService;
        private string userId;
        private string noBusinessMessage;

        public string UserId
        {
            get => userId;
            set
            {
                userId = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToBusinessProfileViewCommand { get; set; }

        public ObservableCollection<Business> Businesses
        {
            get
            {
                UserId = authenticationService.GetIsLoggedIn() ? authenticationService.CurrentUser.Username : string.Empty;
                NoBusinessMessage = UserId == string.Empty ? string.Empty : "You are not managing any businesses";
                return new ObservableCollection<Business>(businessService.GetBusinessesManagedBy(UserId));
            }
        }

        public string NoBusinessMessage
        {
            get
            {
                if (authenticationService.GetIsLoggedIn())
                {
                    return string.Empty;
                }
                else
                {
                    return "Ups.. You have no businesses.";
                }
            }
            set
            {
                noBusinessMessage = value;
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

        public UserManagedBusinessPagesViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            NavigationService = navigationService;
            this.businessService = businessService;
            this.authenticationService = authenticationService;
            UserId = authenticationService.GetIsLoggedIn() ? authenticationService.CurrentUser.Username : string.Empty;
            NavigateToBusinessProfileViewCommand = new RelayCommand(o =>
            {
                if (o is Business business)
                {
                    NavigationService.BusinessId = business.Id;
                    NavigationService.NavigateTo<BusinessProfileViewModel>();
                }
            }, o => true);
        }
    }
}
