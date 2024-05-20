using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;
using Microsoft.Win32;

namespace Bussiness_social_media.MVVM.ViewModel
{
    public class CreateNewBusinessViewModel : Core.ViewModel
    {
        private IBusinessService businessService;
        private INavigationService navigationService;
        private readonly AuthenticationService authenticationService;

        public event EventHandler BusinessCreated;

        private string businessName;
        private string businessDescription;
        private string businessCategory;
        private string phoneNumber;
        private string emailAddress;
        private string website;
        private string address;
        private string logo;
        private string banner;

        public string BusinessName
        {
            get => businessName;
            set
            {
                businessName = value;
                OnPropertyChanged();
            }
        }

        public string BusinessDescription
        {
            get => businessDescription;
            set
            {
                businessDescription = value;
                OnPropertyChanged();
            }
        }

        public string BusinessCategory
        {
            get => businessCategory;
            set
            {
                businessCategory = value;
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

        public string Banner
        {
            get => banner;
            set
            {
                banner = value;
                OnPropertyChanged();
            }
        }

        public string Logo
        {
            get => logo;
            set
            {
                logo = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand AddLogoCommand { get; private set; }
        public ICommand AddBannerCommand { get; private set; }
        public RelayCommand NavigateToHomeViewModelCommand { get; set; }

        public CreateNewBusinessViewModel(INavigationService navigationService, IBusinessService businessService, AuthenticationService authenticationService)
        {
            NavigationService = navigationService;
            this.authenticationService = authenticationService;
            NavigateToHomeViewModelCommand = new RelayCommand(o => { NavigationService.NavigateTo<HomeViewModel>(); }, o => true);
            AddLogoCommand = new RelayCommand(o => { ExecuteAddLogo(); }, o => true);
            AddBannerCommand = new RelayCommand(o => { ExecuteAddBanner(); }, o => true);
            this.businessService = businessService;

            NavigateToHomeViewModelCommand = new RelayCommand(o => { NavigationService.NavigateTo<HomeViewModel>(); }, o => true);
            CreateBusinessCommand = new RelayCommand(CreateBusiness, CanCreateBusiness);
        }

        private void ExecuteAddLogo()
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
                Logo = destinationFilePath;
            }
        }

        private void ExecuteAddBanner()
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
                Logo = destinationFilePath;
            }
        }

        private void CreateBusiness(object parameter)
        {
            List<string> managerUsernames = new List<string> { "admin" };
            if (authenticationService.GetIsLoggedIn())
            {
                managerUsernames.Add(authenticationService.CurrentUser.Username);
                businessService.AddBusiness(BusinessName, BusinessDescription, BusinessCategory, Logo, Banner, PhoneNumber, EmailAddress, Website, Address, DateTime.Now, managerUsernames, new List<int>(), new List<int>(), new List<int>());
            }
            else
            {
                MessageBox.Show("Please log in to create business.");
            }
            navigationService.NavigateTo<HomeViewModel>();
        }

        private bool CanCreateBusiness(object parameter)
        {
            // Add validation logic here if needed
            return true;
        }
    }
}
