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
using System.Windows;

public class PostAndComments
{
    public Post Post { get; set; }
    public ObservableCollection<Comment> Comments
    {
        get;
        set;
    }

    public PostAndComments(Post post, List<Comment> comments)
    {
        Post = post;
        Comments = [.. comments];
    }
}

namespace bussiness_social_media.MVVM.ViewModel
{
    class BusinessProfileViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        private IBusinessService _businessService;
        private readonly AuthenticationService _authenticationService;
        private List<Post> _postList;
        private Business _currentBusiness;
        private bool _isCurrentUserManager;

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

        public IBusinessService BusinessService
        {
            get => _businessService;
            set
            {
                _businessService = value;
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
        public RelayCommand SendCommentCommand { get; set; }
        public RelayCommand NavigateToCreatePostCommand { get; set; }
        public BusinessProfileViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            Navigation = navigationService;
            BusinessService = businessService;
            _authenticationService = authenticationService;
            NavigateToPostsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileViewModel>(); }, o => true);
            NavigateToReviewsCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileReviewsViewModel>(); }, o => true);
            NavigateToContactCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileContactViewModel>();  }, o => true);
            NavigateToAboutCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileAboutViewModel>(); }, o => true);
            NavigateToCreatePostCommand = new RelayCommand(o => { Navigation.NavigateTo<CreatePostViewModel>(); }, o => true);
            //SendCommentCommand = new RelayCommand(o => { Navigation.NavigateTo<BusinessProfileAboutViewModel>(); }, o => true);
            //SendCommentCommand = new RelayCommand(o =>
            //{
            //    MessageBox.Show("Hi! Please stop touching that button ^_^");
            //}, o => true);
            changeCurrrentBusiness();
        }

        

        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts
        {
            get 
            { 
                return GetUpdatedPostsCollection(); 
            }
            set
            {
                _posts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Post> GetUpdatedPostsCollection()
        {
            return new ObservableCollection<Post>(_businessService.GetAllPostsOfBusiness(CurrentBusiness.Id));
        }
        private ObservableCollection<PostAndComments> _postsAndComments;
        public ObservableCollection<PostAndComments> PostsAndComments
        {
            get
            {
                ObservableCollection<PostAndComments> postsAndComments = new ObservableCollection<PostAndComments>();
                foreach (Post post in Posts)
                {
                    postsAndComments.Add(new PostAndComments(post, BusinessService.GetAllCommentsForPost(post.Id)));
                }
                _postsAndComments = postsAndComments;
                return _postsAndComments;
            }
            set
            {
                _postsAndComments = value;
                OnPropertyChanged();
            }
        }

        private string _newCommentContent;
        public string NewCommentContent
        {
            get => _newCommentContent;
            set
            {
                _newCommentContent = value;
                OnPropertyChanged();
            }
        }





        public Business changeCurrrentBusiness()
        {
            return _businessService.GetBusinessById(_navigation.BusinessId);
        }

    }
}
