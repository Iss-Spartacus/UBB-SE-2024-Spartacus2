using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.MVVM.ViewModel
{
    internal class BusinessProfileContactViewModel : Core.ViewModel
    {
        private INavigationService navigation;
        private IBusinessService businessService;
        private AuthenticationService authenticationService;

        public Business CurrentBusiness;
        public FAQ CurrentFAQ;
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
                return CurrentFAQ.Answer;
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
                return changeCurrrentBusiness();
            }
            set
            {
                CurrentBusiness = value;
                OnPropertyChanged(nameof(CurrentBusiness));
            }
        }

        public FAQ CurrentFAQ
        {
            get => CurrentFAQ;
            set
            {
                CurrentFAQ = value;
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
            changeCurrrentBusiness();
            NoFAQ = new FAQ(0, "FAQs...", "--    --\n    \\__/");
            CurrentFAQ = NoFAQ;

            FAQCommand = new RelayCommand(o => {
                if (o is FAQ faq)
                {
                    CurrentFAQ = faq;
                }
                //changeCurrrentFAQ();
            }, o => true);
            // In this class, you have the instance of the business in currentBusiness. You can access it in the BusinessProfileView.xaml but I'm not quite sure how. Ask chat gpt, I tried something and I do not know if it works. It is currently 00:47 and I want to go to sleep
        }

        public Business changeCurrrentBusiness()
        {
            CurrentFAQ = NoFAQ;
            return businessService.GetBusinessById(navigation.BusinessId);
        }

        public FAQ changeCurrrentFAQ()
        {
            List<FAQ> faqList = businessService.GetAllFAQsOfBusiness(CurrentBusiness.Id);
            return faqList[0];
        }
    }
}