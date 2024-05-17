using bussiness_social_media.Core;
using bussiness_social_media.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace bussiness_social_media.Services
{
    public class NavigationParameters
    {
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            _parameters[key] = value;
        }

        public T Get<T>(string key)
        {
            if (_parameters.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }
    }

    public interface INavigationService
    {
        int BusinessId { get; set; }
        ViewModel CurrentView { get; }
        void NavigateTo<T>() where T : ViewModel;


    }
    public class NavigationService : ObservableObject, INavigationService
    {
        private ViewModel _currentView;
        private readonly Func<Type, ViewModel> _viewModelFactory;
        public int BusinessId { get; set; }

        public ViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();    
            }
        }

        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }



    }
}
