using AlgimedWPFApp.ViewModels;
using SQLitePCL;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlgimedWPFApp.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DBContext _context;

    public MainWindow()
    {
        InitializeComponent();

        _context = new DBContext();
        _context.Database.EnsureCreated();
        DataContext = new MainWindowViewModel(_context);
        this.Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _context.Dispose();
    }
}