using business_social_media.Services;
using bussiness_social_media.Core;
using bussiness_social_media.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.IO;

namespace bussiness_social_media.MVVM.ViewModel
{
    public class CreatePostViewModel : Core.ViewModel
    {

        private IBusinessService _businessService;
        private INavigationService _navigationService;
        private readonly AuthenticationService _authenticationService;

        private int _numberOfLikes;
        private DateTime _creationDate;
        private string _imagePath;
        private string _caption;
        private List<int> _commentIds;

        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public RelayCommand CreateBusinessCommand
        {
            get; set;

        }

        public INavigationService NavigationService
        {
            get => _navigationService;
            set
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPhotoCommand { get; private set; }
        public RelayCommand NavigateToHomeViewModelCommand { get; set; }
        public CreatePostViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            NavigationService = navigationService;
            _authenticationService = authenticationService;
            NavigateToHomeViewModelCommand = new RelayCommand(o => { NavigationService.NavigateTo<HomeViewModel>(); }, o => true);
            AddPhotoCommand = new RelayCommand(o => { ExecuteAddPhoto(); }, o => true);
            _businessService = businessService;
            CreateBusinessCommand = new RelayCommand(CreatePost, CanCreatePost);
        }
        private void ExecuteAddPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png;)|*.jpg; *.jpeg; *.png; |All Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string sourceFilePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(openFileDialog.FileName);
                string binDirectory = "\\bin";
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                int index = basePath.IndexOf(binDirectory);
                string pathUntilBin = basePath.Substring(0, index);
                string destinationFilePath = Path.Combine(pathUntilBin, $"Assets\\Images\\" + fileName);


                File.Copy(sourceFilePath, destinationFilePath, true);
                // Placeholder values for logo, banner, etc.

                ImagePath = destinationFilePath;
            }

        }

        private void CreatePost(object parameter)
        {
            if (_authenticationService.getIsLoggedIn())
            {
                _businessService.CreatePostAndAddItToBusiness(_navigationService.BusinessId, ImagePath, Caption);
            }
            else
            {
                MessageBox.Show("You do not have rights to post.");
            }
            _navigationService.NavigateTo<HomeViewModel>();
        }

        private bool CanCreatePost(object parameter)
        {
            // Add real validation logic here if needed
            if (_authenticationService.CurrentUser.Username == "admin")
            {
                return true;
            }
            return false;
        }
    }
}
