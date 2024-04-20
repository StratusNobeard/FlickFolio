using FlickFolio;
using FlickFolio.Windows;
using System;
using System.IO;
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnLinkSeries_Click(object sender, RoutedEventArgs e)
        {

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
