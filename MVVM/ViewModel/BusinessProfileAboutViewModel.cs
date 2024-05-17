using business_social_media.Services;
using bussiness_social_media.Core;
using bussiness_social_media.MVVM.View;
using bussiness_social_media.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussiness_social_media.MVVM.ViewModel
{
    internal class BusinessProfileAboutViewModel : Core.ViewModel
    {

        private INavigationService _navigation;
        private IBusinessService _businessService;
        private AuthenticationService _authenticationService;

        private string _phoneNumber;
        private string _emailAddress;
        private string _website;
        private string _address;
        private string _newAdmin;

        public Business _currentBusiness;

        private bool _isCurrentUserManager;

        private bool _isUpdatingBusinessInfo;

        public bool IsUpdatingBusinessInfo
        {
            get => _isUpdatingBusinessInfo;
            set
            {
                _isUpdatingBusinessInfo = value;
                OnPropertyChanged(nameof(IsUpdatingBusinessInfo));
            }
        }

        public string NewAdmin
        {
            get => _newAdmin;
            set
            {
                _newAdmin = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
            }
        }

        public string Website
        {
            get => _website;
            set
            {
                _website = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentUserManager
        {
            get
            {
                if (_authenticationService.getIsLoggedIn())
                {
                    return _businessService.IsUserManagerOfBusiness(CurrentBusiness.Id,
                        _authenticationService.CurrentUser.Username);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                _isCurrentUserManager = value;
                OnPropertyChanged(nameof(IsCurrentUserManager));
            }
        }

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public Business CurrentBusiness 
        {
            get
            {
                return changeCurrrentBusiness();
            }
            set
            {
                _currentBusiness = value;
                OnPropertyChanged(nameof(CurrentBusiness)); 
            }
        }

        public RelayCommand NavigateToPostsCommand { get; set; }
        public RelayCommand NavigateToReviewsCommand { get; set; }
        public RelayCommand NavigateToContactCommand { get; set; }
        public RelayCommand NavigateToAboutCommand { get; set; }
        public RelayCommand ToggleUpdateFormCommand { get; set; }
        public RelayCommand UpdatePhoneNumberCommand { get; set; }
        public RelayCommand UpdateAddressCommand { get; set; }
        public RelayCommand UpdateEmailCommand { get; set; }
        public RelayCommand UpdateWebsiteCommand { get; set; }
        public RelayCommand AddNewAdministratorCommand { get; set; }

        public BusinessProfileAboutViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            Navigation = navigationService;
            _businessService = businessService;
            _authenticationService = authenticationService;
            NavigateToPostsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileViewModel>(); }, o => true);
            NavigateToReviewsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileReviewsViewModel>(); }, o => true);
            NavigateToContactCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileContactViewModel>(); }, o => true);
            ToggleUpdateFormCommand = new RelayCommand(ToggleUpdateForm, o => true);
            UpdatePhoneNumberCommand = new RelayCommand(UpdatePhoneNumber, o => true);
            UpdateAddressCommand = new RelayCommand(UpdateAddress, o => true);
            UpdateEmailCommand = new RelayCommand(UpdateEmail, o => true);
            UpdateWebsiteCommand = new RelayCommand(UpdateWebsite, o => true);
            AddNewAdministratorCommand = new RelayCommand(AddNewAdministrator, o => true);
            NavigateToAboutCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileAboutViewModel>(); }, o => true);
            changeCurrrentBusiness();
            

        }

        public Business changeCurrrentBusiness()
        {
            return  _businessService.GetBusinessById(_navigation.BusinessId);
        }

        private void AddNewAdministrator(object parameter)
        {
            _businessService.GetBusinessById(CurrentBusiness.Id).AddManager(NewAdmin);
            CurrentBusiness = _businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdatePhoneNumber(object parameter)
        {
           _businessService.GetBusinessById(CurrentBusiness.Id).SetPhoneNumber(PhoneNumber);
           CurrentBusiness = _businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdateAddress(object parameter)
        {
            _businessService.GetBusinessById(CurrentBusiness.Id).SetAddress(Address);
            CurrentBusiness = _businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdateEmail(object parameter)
        {
            _businessService.GetBusinessById(CurrentBusiness.Id).SetEmail(EmailAddress);
            CurrentBusiness = _businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdateWebsite(object parameter)
        {
            _businessService.GetBusinessById(CurrentBusiness.Id).SetWebsite(Website);
            CurrentBusiness = _businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void ToggleUpdateForm(object parameter)
        {
            IsUpdatingBusinessInfo = !IsUpdatingBusinessInfo;
        }
    }
}
