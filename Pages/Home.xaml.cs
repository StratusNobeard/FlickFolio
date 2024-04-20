using FlickFolio.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlickFolio.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();

           
        }

        private void GetCount()
        {
            var db = new FlickFolioContext();

            txtNumberMovies.Text = db.Filmovi.Count().ToString();
            txtNumberSeries.Text = db.Serije.Count().ToString();
            txtNumberActors.Text = db.Glumci.Count().ToString();
            txtNumberDirectors.Text = db.Redatelji.Count().ToString();
            txtNumberGenres.Text = db.Zanrovi.Count().ToString();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetCount();
        }
    }

    
}
