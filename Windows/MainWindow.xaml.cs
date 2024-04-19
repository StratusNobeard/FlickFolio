using System;
using System.Windows;

namespace FlickFolio.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void btnGenres_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new Uri("./Pages/GenreList.xaml", UriKind.Relative));
    }
}
