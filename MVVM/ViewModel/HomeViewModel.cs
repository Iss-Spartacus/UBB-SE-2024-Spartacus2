using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Bussiness_social_media.Core;
using Bussiness_social_media.Services;

namespace Bussiness_social_media.MVVM.ViewModel
{
    public class HomeViewModel : Core.ViewModel
    {
        private INavigationService navigation;
        private IBusinessService businessService;
        private List<Business> businessList;
        private string searchToken;

        private ObservableCollection<Business> businesses;
        public ObservableCollection<Business> Businesses
        {
            get => businesses;
            set
            {
                businesses = value;
                OnPropertyChanged();
            }
        }

        public List<Business> BusinessList
        {
            get => businessList;
            set
            {
                businessList = value;
                OnPropertyChanged();
            }
        }

        public INavigationService NavigationService
        {
            get => navigation;
            set
            {
                navigation = value;
                OnPropertyChanged();
            }
        }

        public string SearchToken
        {
            get => searchToken;
            set
            {
                searchToken = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToCreateNewBusinessViewCommand { get; set; }
        public RelayCommand NavigateToBusinessProfileViewCommand { get; set; }
        public RelayCommand SearchBusinesessCommand { get; set; }

        public HomeViewModel(INavigationService navigationService, IBusinessService businessService)
        {
            searchToken = string.Empty;
            NavigationService = navigationService;
            NavigateToCreateNewBusinessViewCommand = new RelayCommand(o => { NavigationService.NavigateTo<CreateNewBusinessViewModel>(); }, o => true);
            NavigateToBusinessProfileViewCommand = new RelayCommand(o =>
            {
                if (o is Business business)
                {
                    NavigationService.BusinessId = business.Id;
                    NavigationService.NavigateTo<BusinessProfileViewModel>();
                }
            }, o => true);
            this.businessService = businessService;
            businessList = businessService.GetAllBusinesses();
            UpdateBusinessesCollection();
            SearchBusinesessCommand = new RelayCommand(o =>
            {
                BusinessList = businessService.SearchBusinesses(SearchToken);
                UpdateBusinessesCollection();
            }, o => true);
        }

        private void UpdateBusinessesCollection()
        {
            Businesses = new ObservableCollection<Business>(businessList);
        }

        private void NavigateToBusinessProfile(object parameter)
        {
            if (parameter is int businessId)
            {
                NavigationService.NavigateTo<CreateNewBusinessViewModel>();
                NavigationService.BusinessId = businessId;
                NavigateToCreateNewBusinessViewCommand = new RelayCommand(o => { NavigationService.NavigateTo<CreateNewBusinessViewModel>(); }, o => true);
            }
        }
    }
}
