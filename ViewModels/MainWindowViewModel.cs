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
            RegisterCommand = new RelayCommand((a) => RegisterUser());
            LogInCommand = new RelayCommand((a) => LogIn());
            _context = context;
        }

        public RelayCommand RegisterCommand { get; }
        public RelayCommand LogInCommand { get; }

        private void RegisterUser()
        {
            RegisterView regView = new(_context);
            regView.Show();
        }

        private void LogIn()
        {
            LogInView loginView = new(_context);
            loginView.Show();
        }
    }
}