using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness_social_media.MVVM.ViewModel;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.MVVM.ViewModel
{
    internal class BusinessProfileContactViewModel : Core.ViewModel
    {
        private INavigationService navigationService;
        private IBusinessService businessService;
        private AuthenticationService authenticationService;

        public Business MyCurrentBusiness;
        public FAQ MyCurrentFAQ;
        public FAQ NoFAQ;
        private bool isCurrentUserManager;

        public ObservableCollection<FAQ> FAQs
        {
            get
            {
                return new ObservableCollection<FAQ>(businessService.GetAllFAQsOfBusiness(CurrentBusiness.Id));
            }
        }

        public string CurrentFAQAnswer
        {
            get
            {
                return MyCurrentFAQ.Answer;
            }
        }
        public bool IsCurrentUserManager
        {
            get
            {
                if (authenticationService.GetIsLoggedIn())
                {
                    return businessService.IsUserManagerOfBusiness(CurrentBusiness.Id,
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
            get => navigationService;
            set
            {
                navigationService = value;
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
                CurrentBusiness = value;
                OnPropertyChanged(nameof(CurrentBusiness));
            }
        }

        public FAQ CurrentFAQ
        {
            get => MyCurrentFAQ;
            set
            {
                MyCurrentFAQ = value;
                OnPropertyChanged(nameof(CurrentFAQ));
            }
        }

        public RelayCommand NavigateToPostsCommand { get; set; }
        public RelayCommand NavigateToReviewsCommand { get; set; }
        public RelayCommand NavigateToContactCommand { get; set; }
        public RelayCommand NavigateToAboutCommand { get; set; }

        public RelayCommand FAQCommand { get; set; }

        public BusinessProfileContactViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            Navigation = navigationService;
            this.businessService = businessService;
            this.authenticationService = authenticationService;
            NavigateToPostsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileViewModel>(); }, o => true);
            NavigateToReviewsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileReviewsViewModel>(); }, o => true);
            NavigateToContactCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileContactViewModel>(); }, o => true);
            NavigateToAboutCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileAboutViewModel>(); }, o => true);
            ChangeCurrentBusiness();
            NoFAQ = new FAQ(0, "FAQs...", "--    --\n    \\__/");
            CurrentFAQ = NoFAQ;

            FAQCommand = new RelayCommand(o =>
            {
                if (o is FAQ faq)
                {
                    CurrentFAQ = faq;
                }
            }, o => true);
        }

        public Business ChangeCurrentBusiness()
        {
            CurrentFAQ = NoFAQ;
            return businessService.GetBusinessById(navigationService.BusinessId);
        }

        public FAQ ChangeCurrentFAQ()
        {
            List<FAQ> faqList = businessService.GetAllFAQsOfBusiness(CurrentBusiness.Id);
            return faqList[0];
        }
    }
}
