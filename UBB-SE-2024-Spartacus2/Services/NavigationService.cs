using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness_social_media.Core;
using Bussiness_social_media.MVVM.ViewModel;
namespace Bussiness_social_media.Services
{
    public class NavigationParameters
    {
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            parameters[key] = value;
        }

        public T Get<T>(string key)
        {
            if (parameters.TryGetValue(key, out object value))
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
        void NavigateTo<T>()
            where T : ViewModel;
    }
    public class NavigationService : ObservableObject, INavigationService
    {
        private ViewModel currentView;
        private readonly Func<Type, ViewModel> viewModelFactory;
        public int BusinessId { get; set; }

        public ViewModel CurrentView
        {
            get => currentView;
            private set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>()
            where TViewModel : ViewModel
        {
            ViewModel viewModel = viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}
