using FlickFolio.Dialogs;
using FlickFolio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Numerics;
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
    /// Interaction logic for MovieSearch.xaml
    /// </summary>
    public partial class MovieSearch : Page
    {
        public MovieSearch()
        {
            InitializeComponent();

            InitializeDropDowns();
        }



        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            string title = tbName.Text;

            int? duration = string.IsNullOrWhiteSpace(tbDuration.Text) ? null : int.Parse(tbDuration.Text);
            int? year = string.IsNullOrWhiteSpace(tbYear.Text) ? null : int.Parse(tbYear.Text);

            int? directorId = cmbDirectors.SelectedIndex > 0 ? (cmbDirectors.SelectedItem as Redatelj).Id : null;

            int? genreId = cmbGenres.SelectedIndex > 0 ? (cmbGenres.SelectedItem as Zanr).Id : null;

            int? actorId = cmbActors.SelectedIndex > 0 ? (cmbActors.SelectedItem as Glumac).Id : null;

            List<Film> searchResults = SearchFilms(title, duration, year, directorId, genreId, actorId);

            using var db = new FlickFolioContext();

            lbMovie.ItemsSource = searchResults;

        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            GetInfo();
        }


        private void CheckIfNumber(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
            else
            {
                TextBox textBox = (TextBox)sender;
                string newText = textBox.Text.Substring(0, textBox.SelectionStart) + e.Text +
                                  textBox.Text.Substring(textBox.SelectionStart + textBox.SelectionLength);

                if (string.IsNullOrWhiteSpace(newText) || newText == "0" || !int.TryParse(newText, out int number))
                {
                    e.Handled = true;
                }
            }
        }

        private void InitializeDropDowns()
        {


            using var db = new FlickFolioContext();

            var directors = db.Redatelji.ToList();
            var actors = db.Glumci.ToList();
            var genres = db.Zanrovi.ToList();

            var noSelectionDirector = new Redatelj { Ime = "...", Prezime = "" };
            var noSelectionActor = new Glumac { Ime = "...", Prezime = "" };
            var noSelectionGenre = new Zanr { Naziv = "..." };

            directors.Insert(0, noSelectionDirector);
            actors.Insert(0, noSelectionActor);
            genres.Insert(0, noSelectionGenre);

            cmbDirectors.ItemsSource = directors;
            cmbActors.ItemsSource = actors;
            cmbGenres.ItemsSource = genres;
        }


        public List<Film> SearchFilms(string title, int? duration, int? year, int? directorId, int? genreId, int? actorId)
        {
            using var db = new FlickFolioContext();

            var query = db.Filmovi.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(f => f.Naziv.Contains(title));
            }

            if (duration.HasValue)
            {
                query = query.Where(f => f.Trajanje == duration.Value);
            }

            if (year.HasValue)
            {
                query = query.Where(f => f.Godina == year.Value);
            }

            if (directorId.HasValue && directorId != -1)
            {
                query = query.Where(f => f.RedateljId == directorId.Value);
            }

            if (genreId.HasValue && genreId != -1)
            {
                query = query.Where(f => f.FilmZanr.Any(fz => fz.ZanrId == genreId.Value));
            }

            if (actorId.HasValue && actorId != -1)
            {
                query = query.Where(f => f.FilmGlumac.Any(fg => fg.GlumacId == actorId.Value));
            }

            return query.ToList();
        }

        private void cmbDirectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDirectors.SelectedIndex == 0)
            {
                cmbDirectors.SelectedIndex = -1;
            }
        }

        private void cmbActors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbActors.SelectedIndex == 0)
            {
                cmbActors.SelectedIndex = -1;
            }
        }

        private void cmbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGenres.SelectedIndex == 0)
            {
                cmbGenres.SelectedIndex = -1;
            }
        }

        private void lbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnInfo.IsEnabled = lbMovie.SelectedItem != null;
        }



        private void lbMovie_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GetInfo();
        }

        public void GetInfo()
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
