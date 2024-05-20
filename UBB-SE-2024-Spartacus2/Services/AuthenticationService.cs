using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Bussiness_social_media.MVVM.Model.Repository;
using Newtonsoft.Json;

namespace Bussiness_social_media.Services
{
    public class AuthenticationService
    {
        private IUserRepository userRepository;
        private Dictionary<string, DateTime> sessionTokens;
        private bool isLoggedIn;
        private Timer logoutTimer;
        private int sessionDurationSeconds;
        private Account currentUser;
        public event EventHandler<EventArgs> LoginStatusChanged;
        public Account CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
            }
        }

        public AuthenticationService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            sessionTokens = new Dictionary<string, DateTime>();
            isLoggedIn = false;
            sessionDurationSeconds = 10;
        }

        public bool GetIsLoggedIn()
        {
            return isLoggedIn;
        }

        public bool AuthenticateUser(string username, string password)
        {
            password = userRepository.GetMd5Hash(password);

            if (userRepository.IsCredentialsValid(username, password))
            {
                string sessionToken = Guid.NewGuid().ToString();
                sessionTokens.Add(sessionToken, DateTime.Now);
                isLoggedIn = true;

                CurrentUser = new Account(username, password);

                LoginStatusChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LogoutUser(object state)
        {
            isLoggedIn = false;
            Console.WriteLine("\nUser has been logged out automatically.");
            LoginStatusChanged?.Invoke(this, EventArgs.Empty); // Raise the event
        }

        public List<Account> GetAllUsers()
        {
            return userRepository.GetAllUsers();
        }
    }
}

