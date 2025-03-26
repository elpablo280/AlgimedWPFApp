using AlgimedWPFApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlgimedWPFApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly DBContext _context;

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(DBContext context)
        {
            RegisterCommand = new RelayCommand(async (a) => await RegisterUser());
            LogInCommand = new RelayCommand(async (a) => await LogIn());
            _context = context;
        }

        public RelayCommand RegisterCommand { get; }
        public RelayCommand LogInCommand { get; }

        private async Task<string> RegisterUser()   // todo
        {
            RegisterView regView = new(_context);
            regView.Show();

            return "";
        }

        private async Task<string> LogIn()   // todo
        {
            LogInView loginView = new(_context);
            loginView.Show();

            return "";
        }
    }
}