using FlickFolio.Dialogs;
using FlickFolio.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void lbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                MovieId = null,
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

            var film = lbMovie.SelectedItem as dynamic;
            if (film != null)
            {
                dialog.MovieId = film.Id;

                var result = dialog.ShowDialog();
                if (result.Value)
                {
                    RefreshGrid();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Sigurni ste da želite izbrisati podatak?", "Potvrda", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                var item = lbMovie.SelectedItem as dynamic;
                int selectedItemId = item.Id;

                using var db = new FlickFolioContext();
                
                var connection = db.Connection;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "PRAGMA foreign_keys = 0;";
                command.ExecuteNonQuery();

                var film = db.Filmovi
                    .Where(x => x.Id == selectedItemId)
                    .First();

                db.Filmovi.Remove(film);

                db.SaveChanges();

                RefreshGrid();
            }
        }


        private void lbMovie_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var item = lbMovie.SelectedItem as dynamic;
            int selectedItemId = item.Id;

            using var db = new FlickFolioContext();

            var film = db.Filmovi
                        .Where(x => x.Id == selectedItemId)
                        .FirstOrDefault();

            if (film != null)
            {
                var dialog = new InfoWindow(ShowFilmInfo(film, db), ShowFilmInfo(film, db).Split('\n').Length)
                {
                    Owner = Parent as Window
                };

                dialog.Show();
            }
        }

        public static string ShowFilmInfo(Film film, FlickFolioContext db)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Naziv: {film.Naziv}");
            sb.AppendLine($"Trajanje: {film.Trajanje} min");
            sb.AppendLine($"Godina: {film.Godina}");

            var glumci = db.FilmGlumac
                            .Where(fg => fg.FilmId == film.Id)
                            .Select(fg => $"{fg.Glumac.Ime} {fg.Glumac.Prezime}");

            var redatelj = db.Redatelji.FirstOrDefault(r => r.Id == film.RedateljId);
            if (redatelj != null)
            {
                sb.AppendLine($"Redatelj: {redatelj.Ime} {redatelj.Prezime}");
            }
            else
            {
                sb.AppendLine("Redatelj nije pronađen.");
            }


            sb.AppendLine("Glumci:");
            foreach (var glumac in glumci)
            {
                sb.AppendLine($"- {glumac}");
            }

            var zanrovi = db.FilmZanr
                            .Where(fz => fz.FilmId == film.Id)
                            .Select(fz => fz.Zanr.Naziv);

            sb.AppendLine("Žanrovi:");
            foreach (var zanr in zanrovi)
            {
                sb.AppendLine($"- {zanr}");
            }

            return sb.ToString();
        }


    }
}
