using AlgimedWPFApp.Models;
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
            LogInCommand = new RelayCommand(async (a) => await LogIn(Login, Password));
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

        public async Task<bool> LogIn(string login, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Login == login);
            if (user == null)
            {
                MessageBox.Show("No such user found");
                return false;
            }

            if (VerifyPassword(password, user.Password))
            {
                CloseWindow();
                return true;
            }
            else
            {
                MessageBox.Show("Invalid password");
                return false;
            }
        }

        private bool VerifyPassword(string enteredPassword, string password) // todo
        {
            // Проверка хэшированного пароля
            //return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return enteredPassword == password;
        }
    }
}
