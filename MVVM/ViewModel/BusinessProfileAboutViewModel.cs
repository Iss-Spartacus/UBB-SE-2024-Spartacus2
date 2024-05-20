using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;
using Bussiness_social_media.MVVM.ViewModel;

namespace Bussiness_social_media.MVVM.ViewModel
{
    internal class BusinessProfileAboutViewModel : Bussiness_social_media.Core.ViewModel
    {
        private INavigationService navigation;
        private IBusinessService businessService;
        private AuthenticationService authenticationService;

        private string phoneNumber;
        private string emailAddress;
        private string website;
        private string address;
        private string newAdmin;
        private Business currentBusiness;
        private bool isCurrentUserManager;
        private bool isUpdatingBusinessInfo;

        public bool IsUpdatingBusinessInfo
        {
            get => isUpdatingBusinessInfo;
            set
            {
                isUpdatingBusinessInfo = value;
                OnPropertyChanged(nameof(IsUpdatingBusinessInfo));
            }
        }

        public string NewAdmin
        {
            get => newAdmin;
            set
            {
                newAdmin = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string EmailAddress
        {
            get => emailAddress;
            set
            {
                emailAddress = value;
                OnPropertyChanged();
            }
        }

        public string Website
        {
            get => website;
            set
            {
                website = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentUserManager
        {
            get
            {
                if (authenticationService.GetIsLoggedIn())
                {
                    return businessService.IsUserManagerOfBusiness(currentBusiness.Id,
                        authenticationService.CurrentUser.Username);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                isCurrentUserManager = value;
                OnPropertyChanged(nameof(IsCurrentUserManager));
            }
        }

        public INavigationService Navigation
        {
            get => navigation;
            set
            {
                navigation = value;
                OnPropertyChanged();
            }
        }

        public Business CurrentBusiness
        {
            get
            {
                return ChangeCurrentBusiness();
            }
            set
            {
                currentBusiness = value;
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
            this.businessService = businessService;
            this.authenticationService = authenticationService;
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
            ChangeCurrentBusiness();
        }

        public Business ChangeCurrentBusiness()
        {
            return businessService.GetBusinessById(navigation.BusinessId);
        }

        private void AddNewAdministrator(object parameter)
        {
            businessService.GetBusinessById(CurrentBusiness.Id).AddManager(NewAdmin);
            CurrentBusiness = businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdatePhoneNumber(object parameter)
        {
            businessService.GetBusinessById(CurrentBusiness.Id).SetPhoneNumber(PhoneNumber);
            CurrentBusiness = businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdateAddress(object parameter)
        {
            businessService.GetBusinessById(CurrentBusiness.Id).SetAddress(Address);
            CurrentBusiness = businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdateEmail(object parameter)
        {
            businessService.GetBusinessById(CurrentBusiness.Id).SetEmail(EmailAddress);
            CurrentBusiness = businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void UpdateWebsite(object parameter)
        {
            businessService.GetBusinessById(CurrentBusiness.Id).SetWebsite(Website);
            CurrentBusiness = businessService.GetBusinessById(CurrentBusiness.Id);
        }

        private void ToggleUpdateForm(object parameter)
        {
            IsUpdatingBusinessInfo = !IsUpdatingBusinessInfo;
        }
    }
}
