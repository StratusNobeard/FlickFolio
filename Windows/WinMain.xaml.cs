using System;
using System.Windows;
using System.Windows.Input;

namespace FlickFolio.Prozori
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class WinMain : Window
    {


        public WinMain()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnLinkHome_Click(object sender, RoutedEventArgs e)
        {
            frMainFrame.Navigate(new Uri("./Pages/Home.xaml", UriKind.Relative));
        }

        private void btnLinkMovies_Click(object sender, RoutedEventArgs e)
        {
            frMainFrame.Navigate(new Uri("./Pages/MovieList.xaml", UriKind.Relative));
        }

        private void btnLinkSeries_Click(object sender, RoutedEventArgs e)
        {
            frMainFrame.Navigate(new Uri("./Pages/SeriesList.xaml", UriKind.Relative));
        }

        private void btnLinkGenres_Click(object sender, RoutedEventArgs e)
        {
            frMainFrame.Navigate(new Uri("./Pages/GenreList.xaml", UriKind.Relative));
        }

        private void btnLinkActors_Click(object sender, RoutedEventArgs e)
        {
            frMainFrame.Navigate(new Uri("./Pages/ActorList.xaml", UriKind.Relative));
        }

        private void btnLinkDirectors_Click(object sender, RoutedEventArgs e)
        {
            frMainFrame.Navigate(new Uri("./Pages/DirectorList.xaml", UriKind.Relative));

        }
    }
}
