using AlgimedWPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AlgimedWPFApp.Views
{
    /// <summary>
    /// Interaction logic for LogInView.xaml
    /// </summary>
    public partial class LogInView : Window
    {
        public LogInView(DBContext context)
        {
            InitializeComponent();
            var viewModel = new LogInViewModel(context);
            DataContext = viewModel;
            viewModel.RequestClose += (sender, e) => this.Close();
        }
    }
}
