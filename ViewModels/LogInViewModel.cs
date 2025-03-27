using AlgimedWPFApp.Models;
using AlgimedWPFApp.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgimedWPFApp.ViewModels
{
    class LogInViewModel : BaseViewModel
    {
        private readonly DBContext _context;

        public LogInViewModel(DBContext context)
        {
            _context = context;
            LogInCommand = new RelayCommand((a) => LogIn(Login, Password));
        }

        public RelayCommand LogInCommand { get; }

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public event EventHandler RequestClose;

        private void CloseWindow()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        public bool LogIn(string login, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Login == login);
            if (user == null)
            {
                MessageBox.Show("No such user found");
                return false;
            }

            if (VerifyPassword(password, user.Password))
            {
                DataView dataView = new(_context);
                dataView.Show();
                CloseWindow();
                return true;
            }
            else
            {
                MessageBox.Show("Invalid password");
                return false;
            }
        }

        // Проверка хэшированного пароля
        private static bool VerifyPassword(string enteredPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, password);
        }
    }
}
