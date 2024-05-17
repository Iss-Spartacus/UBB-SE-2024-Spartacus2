using business_social_media.Services;
using bussiness_social_media.Core;
using bussiness_social_media.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bussiness_social_media.MVVM.ViewModel
{
    internal class BusinessProfileContactViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        private IBusinessService _businessService;
        private AuthenticationService _authenticationService;

        public Business _currentBusiness;
        public FAQ _currentFAQ;
        public FAQ _noFAQ;


        private bool _isCurrentUserManager;

        public ObservableCollection<FAQ> FAQs
        {
            get
            {

                return new ObservableCollection<FAQ>(_businessService.GetAllFAQsOfBusiness(CurrentBusiness.Id));
            }
        }

        public string CurrentFAQAnswer
        {
            get
            {
                return _currentFAQ.Answer;
            }
        }


        public bool IsCurrentUserManager
        {
            get
            {
                if (_authenticationService.GetIsLoggedIn())
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

        public FAQ CurrentFAQ
        {
            get => _currentFAQ;
            set
            {
                _currentFAQ = value;
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
            _businessService = businessService;
            _authenticationService = authenticationService;
            NavigateToPostsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileViewModel>(); }, o => true);
            NavigateToReviewsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileReviewsViewModel>(); }, o => true);
            NavigateToContactCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileContactViewModel>(); }, o => true);
            NavigateToAboutCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileAboutViewModel>(); }, o => true);
            changeCurrrentBusiness();
            _noFAQ = new FAQ(0, "FAQs...", "--    --\n    \\__/");
            _currentFAQ = _noFAQ;

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
            CurrentFAQ = _noFAQ;
            return _businessService.GetBusinessById(_navigation.BusinessId);
        }

        public FAQ changeCurrrentFAQ()
        {
            List<FAQ> faqList = _businessService.GetAllFAQsOfBusiness(CurrentBusiness.Id);
            return faqList[0];
        }
    }
}
