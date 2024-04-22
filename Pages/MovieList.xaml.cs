using FlickFolio.Dialogs;
using FlickFolio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for MovieList.xaml
    /// </summary>
    public partial class MovieList : Page
    {
        public MovieList()
        {
            InitializeComponent();

            RefreshGrid();
        }

        private void lbActor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = lbMovie.SelectedItem != null;
            btnDelete.IsEnabled = lbMovie.SelectedItem != null;
        }

        private void RefreshGrid()
        {
            using var db = new FlickFolioContext();

            var x = db.Filmovi
                .Include(r => r.Redatelj)
                .Select(_ => new
                {
                    _.Id,
                    _.Naziv,
                    _.Trajanje,
                    _.Godina,
                    RedateljImePrezime = $"{_.Redatelj.Ime} {_.Redatelj.Prezime}"
                });

            lbMovie.ItemsSource = x.ToList();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MovieDetails
            {
                Model = null,
                Owner = Parent as Window
            };

            var result = dialog.ShowDialog();
            if (result.Value)
            {
                RefreshGrid();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MovieDetails
            {
                Owner = Parent as Window
            };

            dialog.Model = lbMovie.SelectedItem as Film;

            var result = dialog.ShowDialog();
            if (result.Value)
            {
                RefreshGrid();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Sigurni ste da želite izbrisati podatak?", "Potvrda", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                using var db = new FlickFolioContext();

                var item = lbMovie.SelectedItem as Film;
               
                db.Filmovi.Remove(item);
                db.SaveChanges();

                RefreshGrid();
            }
        }
    }
}
