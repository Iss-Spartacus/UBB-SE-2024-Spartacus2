using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;
using Bussiness_social_media.MVVM.ViewModel;

public class PostAndComments
{
    public Post Post { get; set; }
    public ObservableCollection<Comment> Comments { get; set; }

    public PostAndComments(Post post, List<Comment> comments)
    {
        Post = post;
        Comments = new ObservableCollection<Comment>(comments);
    }
}

namespace Bussiness_social_media.MVVM.ViewModel
{
    internal class BusinessProfileViewModel : Core.ViewModel
    {
        private INavigationService navigationService;
        private IBusinessService businessService;
        private AuthenticationService authenticationService;
        private List<Post> postList;
        public Business MyCurrentBusiness;
        private bool isCurrentUserManager;

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

        public IBusinessService BusinessService
        {
            get => businessService;
            set
            {
                businessService = value;
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
                MyCurrentBusiness = value;
                OnPropertyChanged(nameof(CurrentBusiness));
            }
        }

        public RelayCommand NavigateToPostsCommand { get; set; }
        public RelayCommand NavigateToReviewsCommand { get; set; }
        public RelayCommand NavigateToContactCommand { get; set; }
        public RelayCommand NavigateToAboutCommand { get; set; }
        public RelayCommand SendCommentCommand { get; set; }
        public RelayCommand NavigateToCreatePostCommand { get; set; }

        public BusinessProfileViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            Navigation = navigationService;
            BusinessService = businessService;
            this.authenticationService = authenticationService;
            NavigateToPostsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileViewModel>(); }, o => true);
            NavigateToReviewsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileReviewsViewModel>(); }, o => true);
            NavigateToContactCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileContactViewModel>(); }, o => true);
            NavigateToAboutCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileAboutViewModel>(); }, o => true);
            NavigateToCreatePostCommand = new RelayCommand(o => { Navigation.NavigateTo<CreatePostViewModel>(); }, o => true);
            ChangeCurrentBusiness();
        }

        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts
        {
            get
            {
                return GetUpdatedPostsCollection();
            }
            set
            {
                posts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Post> GetUpdatedPostsCollection()
        {
            return new ObservableCollection<Post>(businessService.GetAllPostsOfBusiness(CurrentBusiness.Id));
        }

        private ObservableCollection<PostAndComments> postsAndComments;
        public ObservableCollection<PostAndComments> PostsAndComments
        {
            get
            {
                ObservableCollection<PostAndComments> postsAndComments = new ObservableCollection<PostAndComments>();
                foreach (Post post in Posts)
                {
                    postsAndComments.Add(new PostAndComments(post, BusinessService.GetAllCommentsForPost(post.Id)));
                }
                this.postsAndComments = postsAndComments;
                return this.postsAndComments;
            }
            set
            {
                postsAndComments = value;
                OnPropertyChanged();
            }
        }

        private string newCommentContent;
        public string NewCommentContent
        {
            get => newCommentContent;
            set
            {
                newCommentContent = value;
                OnPropertyChanged();
            }
        }

        public Business ChangeCurrentBusiness()
        {
            return businessService.GetBusinessById(navigationService.BusinessId);
        }
    }
}
