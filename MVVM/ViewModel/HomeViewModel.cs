using bussiness_social_media.Core;
using bussiness_social_media.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace bussiness_social_media.MVVM.ViewModel
{
    public class HomeViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        private IBusinessService _businessService;
        private List<Business> _businessList;

        private string _searchToken;


        private ObservableCollection<Business> _businesses;
        public ObservableCollection<Business> Businesses
        {
            get => _businesses;
            set
            {
                _businesses = value;
                OnPropertyChanged();
            }
        }

        private void UpdateBusinessesCollection()
        {
            Businesses = new ObservableCollection<Business>(_businessList);
        }

        public List<Business> BusinessList
        {
            get => _businessList;
            set
            {
                _businessList = value;
                OnPropertyChanged();
            }
        }

        public INavigationService NavigationService
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public string SearchToken
        {
            get => _searchToken;
            set
            {
                _searchToken = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToCreateNewBusinessViewCommand { get; set; }
        public RelayCommand NavigateToBusinessProfileViewCommand { get; set; }

        public RelayCommand SearchBusinesessCommand { get; set; }


        public HomeViewModel(INavigationService navigationService, IBusinessService businessService)
        {
            _searchToken = string.Empty;
            NavigationService = navigationService;
            NavigateToCreateNewBusinessViewCommand = new RelayCommand(o => { NavigationService.NavigateTo<CreateNewBusinessViewModel>(); }, o => true);

            //NavigateToBusinessProfileViewCommand = new RelayCommand(o => { NavigationService.NavigateTo<BusinessProfileViewModel>(); }, o => true);
            NavigateToBusinessProfileViewCommand = new RelayCommand(o =>
            {
                if (o is Business business)
                {
                    NavigationService.BusinessId = business.Id; 
                    NavigationService.NavigateTo<BusinessProfileViewModel>();
                }
            }, o => true);
            _businessService = businessService;
            _businessList = _businessService.GetAllBusinesses();
            UpdateBusinessesCollection();

            SearchBusinesessCommand = new RelayCommand(o => {
                BusinessList = _businessService.SearchBusinesses(SearchToken);
                UpdateBusinessesCollection();
                }, o => true);
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
