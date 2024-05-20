using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.MVVM.ViewModel
{
    public class CreatePostViewModel : Core.ViewModel
    {
        private IBusinessService businessService;
        private INavigationService navigationService;
        private readonly AuthenticationService authenticationService;

        private int numberOfLikes;
        private DateTime creationDate;
        private string imagePath;
        private string caption;
        private List<int> commentIds;

        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public RelayCommand CreateBusinessCommand { get; set; }

        public INavigationService NavigationService
        {
            get => navigationService;
            set
            {
                navigationService = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPhotoCommand { get; private set; }
        public RelayCommand NavigateToHomeViewModelCommand { get; set; }

        public CreatePostViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            NavigationService = navigationService;
            this.authenticationService = authenticationService;
            NavigateToHomeViewModelCommand = new RelayCommand(o => { NavigationService.NavigateTo<HomeViewModel>(); }, o => true);
            AddPhotoCommand = new RelayCommand(o => { ExecuteAddPhoto(); }, o => true);
            this.businessService = businessService;
            CreateBusinessCommand = new RelayCommand(CreatePost, CanCreatePost);
            ImagePath = string.Empty;
            Caption = string.Empty;
            commentIds = new List<int>();
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
                ImagePath = destinationFilePath;
            }
        }

        private void CreatePost(object parameter)
        {
            if (authenticationService.GetIsLoggedIn())
            {
                businessService.CreatePostAndAddItToBusiness(navigationService.BusinessId, ImagePath, Caption);
            }
            else
            {
                MessageBox.Show("You do not have rights to post.");
            }
            navigationService.NavigateTo<HomeViewModel>();
        }

        private bool CanCreatePost(object parameter)
        {
            // Add real validation logic here if needed
            if (authenticationService.CurrentUser.Username == "admin")
            {
                return true;
            }
            return false;
        }
    }
}
