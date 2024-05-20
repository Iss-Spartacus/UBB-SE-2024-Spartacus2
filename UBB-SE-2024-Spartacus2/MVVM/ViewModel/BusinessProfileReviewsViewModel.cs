using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;
using Bussiness_social_media.MVVM.ViewModel;
namespace Bussiness_social_media.MVVM.ViewModel
{
    public class BusinessProfileReviewsViewModel : Core.ViewModel
    {
        private INavigationService navigation;
        private IBusinessService businessService;
        private readonly AuthenticationService authenticationService;
        private Business currentBusiness;
        private Review currentReview;
        private string imagePath;
        private List<Review> reviewsList;

        // Property to hold the image source of the current business
        private ImageSource businessImage;
        private string comment;
        private bool isCurrentUserManager;
        private string title;
        private int rating;

        public int Rating
        {
            get => rating;
            set
            {
                rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public List<Review> ReviewsList
        {
            get => reviewsList;
            set
            {
                reviewsList = value;
                OnPropertyChanged();
            }
        }

        public bool IsCurrentUserManager
        {
            get
            {
                if (authenticationService.GetIsLoggedIn())
                {
                    return !businessService.IsUserManagerOfBusiness(currentBusiness.Id,
                        authenticationService.CurrentUser.Username);
                }
                else
                {
                    return true;
                }
            }
            set
            {
                isCurrentUserManager = value;
                OnPropertyChanged(nameof(IsCurrentUserManager));
            }
        }
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public ImageSource BusinessImage
        {
            get
            {
                return new BitmapImage(new Uri(currentBusiness.Logo));
            }
            set
            {
                businessImage = value;
                OnPropertyChanged(nameof(BusinessImage));
            }
        }

        public Business CurrentBusiness
        {
            get { return ChangeCurrentBusiness(); }
        }

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged();
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

        public RelayCommand NavigateToPostsCommand { get; set; }
        public RelayCommand NavigateToReviewsCommand { get; set; }
        public RelayCommand NavigateToContactCommand { get; set; }
        public RelayCommand NavigateToAboutCommand { get; set; }
        public RelayCommand LeaveReviewCommand { get; set; }
        public RelayCommand AddImageCommand { get; private set; }

        public BusinessProfileReviewsViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            Navigation = navigationService;
            this.businessService = businessService;
            this.authenticationService = authenticationService;
            NavigateToPostsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileViewModel>(); }, o => true);
            NavigateToReviewsCommand = new RelayCommand(o => { Navigation.NavigateTo<CreateNewBusinessViewModel>(); }, o => true);
            NavigateToContactCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileContactViewModel>(); }, o => true);
            NavigateToAboutCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileAboutViewModel>(); }, o => true);
            AddImageCommand = new RelayCommand(o => { ExecuteAddImage(); }, o => true);
            LeaveReviewCommand = new RelayCommand(o => { LeaveReview(); }, o => true);
            currentBusiness = ChangeCurrentBusiness();
            ImageSource img = new BitmapImage(new Uri(currentBusiness.Logo));
            BusinessImage = img;

            reviewsList = businessService.GetAllReviewsForBusiness(CurrentBusiness.Id);
        }
        private void LeaveReview()
        {
            if (authenticationService.GetIsLoggedIn())
            {
                string userName = authenticationService.CurrentUser.Username;
                int businessId = currentBusiness.Id;
                int rating = Rating;
                string comment = Comment;
                string title = Title;
                string imagePath = ImagePath;
                businessService.CreateReviewAndAddItToBusiness(businessId, userName, rating, comment, title, imagePath);
                reviewsList = businessService.GetAllReviewsForBusiness(businessId);
                OnPropertyChanged(nameof(ReviewsList));
            }
            else
            {
                MessageBox.Show("Please log in to leave a review.");
            }
        }
        public Business ChangeCurrentBusiness()
        {
            return businessService.GetBusinessById(navigation.BusinessId);
        }

        private void ExecuteAddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png;)|*.jpg; *.jpeg; *.png; |All Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string filename = openFileDialog.FileName;
                ImagePath = filename;
            }
        }
    }
}
