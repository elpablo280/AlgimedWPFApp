using AlgimedWPFApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static AlgimedWPFApp.ViewModels.MainWindowViewModel;

namespace AlgimedWPFApp.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private readonly DBContext _context;

        public RegisterViewModel(DBContext context)
        {
            _context = context;
            RegisterCommand = new RelayCommand(async (a) => await Register(Login, Password));
        }

        public RelayCommand RegisterCommand { get; }

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

        public async Task<bool> Register(string login, string password)
        {
            // Check for unique login
            if (await _context.Users.AnyAsync(u => u.Login == login))
            {
                MessageBox.Show("Login already exists");
                return false;
            }

            // Check password against requirements
            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password does not meet requirements: >= 6 symbols, >= 1 letter, >= 1 number");
                return false;
            }

            var user = new User { Login = login, Password = password };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            MessageBox.Show("New user registered!");
            CloseWindow();
            return true;
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6 && password.Any(char.IsDigit) && password.Any(char.IsLetter);
        }
    }
}
