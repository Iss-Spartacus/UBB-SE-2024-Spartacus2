using System.IO;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;
using System.Xml;
using Bussiness_social_media.MVVM.Model.Repository;
using Bussiness_social_media.Services;
using Bussiness_social_media.Core;
using Bussiness_social_media.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
namespace Bussiness_social_media
{
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            // Read XML file path from configuration
            string businessesXmlFilePath = ConfigurationManager.AppSettings["BusinessesXmlFilePath"];
            string usersXmlFilePath = ConfigurationManager.AppSettings["UsersXmlFilePath"];
            string postsXmlFilePath = ConfigurationManager.AppSettings["PostsXmlFilePath"];
            string reviewsXmlFilePath = ConfigurationManager.AppSettings["ReviewsXmlFilePath"];
            string faqsXmlFilePath = ConfigurationManager.AppSettings["FAQsXmlFilePath"];
            string commentsXmlFilePath = ConfigurationManager.AppSettings["CommentsXmlFilePath"];
            string binDirectory = "\\bin";
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string pathUntilBin;
            int index = basePath.IndexOf(binDirectory);
            pathUntilBin = basePath.Substring(0, index);
            businessesXmlFilePath = Path.Combine(pathUntilBin, businessesXmlFilePath);
            usersXmlFilePath = Path.Combine(pathUntilBin, usersXmlFilePath);
            postsXmlFilePath = Path.Combine(pathUntilBin, postsXmlFilePath);
            reviewsXmlFilePath = Path.Combine(pathUntilBin, reviewsXmlFilePath);
            faqsXmlFilePath = Path.Combine(pathUntilBin, faqsXmlFilePath);
            commentsXmlFilePath = Path.Combine(pathUntilBin, commentsXmlFilePath);

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<IBusinessService, BusinessService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<CreateNewBusinessViewModel>();
            services.AddSingleton<UserManagedBusinessPagesViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<BusinessProfileViewModel>();
            services.AddSingleton<IFAQService, FAQService>();
            services.AddSingleton<IPostService, PostService>();
            services.AddSingleton<IReviewService, ReviewService>();
            services.AddSingleton<ICommentService, CommentService>();

            // Pass xmlFilePath to your BusinessRepository constructor
            services.AddSingleton<IBusinessRepository>(provider => new BusinessRepository(businessesXmlFilePath));
            services.AddSingleton<IUserRepository>(provider => new UserRepository(usersXmlFilePath));
            services.AddSingleton<IPostRepository>(provider => new PostRepository(postsXmlFilePath));
            services.AddSingleton<IReviewRepository>(provider => new ReviewRepository(reviewsXmlFilePath));
            services.AddSingleton<IFAQRepository>(provider => new FAQRepository(faqsXmlFilePath));
            services.AddSingleton<ICommentRepository>(provider => new CommentRepository(commentsXmlFilePath));

            services.AddSingleton<BusinessProfileViewModel>();
            services.AddSingleton<BusinessProfileReviewsViewModel>();
            services.AddSingleton<BusinessProfileContactViewModel>();
            services.AddSingleton<BusinessProfileAboutViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<RegisterViewModel>();
            services.AddSingleton<CreatePostViewModel>();
            // Delegation
            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // var loginWindow = new Window();
            // loginWindow.Content = _serviceProvider.GetRequiredService<LoginViewModel>();
            // loginWindow.ShowDialog();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
