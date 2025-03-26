using AlgimedWPFApp.Models;
using AlgimedWPFApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgimedWPFApp.Views
{
    public partial class RegisterView : Window
    {
        public RegisterView(DBContext context)
        {
            InitializeComponent();
            var viewModel = new RegisterViewModel(context);
            DataContext = viewModel;
            viewModel.RequestClose += (sender, e) => this.Close();
        }
    }
}
