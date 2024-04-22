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
    /// Interaction logic for SeriesList.xaml
    /// </summary>
    public partial class SeriesList : Page
    {
        public SeriesList()
        {
            InitializeComponent();

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            using var db = new FlickFolioContext();

            var x = db.Serije
                .Include(r => r.Redatelj)
                .Select(_ => new
                {
                    _.Id,
                    _.Naziv,
                    Trajanje = $"{_.PocetnaGodina}-{_.ZavrsnaGodina}",
                    _.BrojSezona,
                    RedateljImePrezime = $"{_.Redatelj.Ime} {_.Redatelj.Prezime}"
                });

            lbSeries.ItemsSource = x.ToList();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SeriesDetails
            {
                SeriesId = null,
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
            var dialog = new SeriesDetails
            {
                Owner = Parent as Window
            };

            var serija = lbSeries.SelectedItem as dynamic;
            if (serija != null)
            {
                dialog.SeriesId = serija.Id;

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
                var item = lbSeries.SelectedItem as dynamic;
                int selectedItemId = item.Id;

                using var db = new FlickFolioContext();

                var connection = db.Connection;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "PRAGMA foreign_keys = 0;";
                command.ExecuteNonQuery();

                var serija = db.Serije
                    .Where(x => x.Id == selectedItemId)
                    .First();

                db.Serije.Remove(serija);

                db.SaveChanges();

                RefreshGrid();
            }
        }

        private void lbSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = lbSeries.SelectedItem != null;
            btnDelete.IsEnabled = lbSeries.SelectedItem != null;
        }

        private void lbSeries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = lbSeries.SelectedItem as dynamic;
            int selectedItemId = item.Id;

            using var db = new FlickFolioContext();

            var serija = db.Serije
                        .Where(x => x.Id == selectedItemId)
                        .FirstOrDefault();

            if (serija != null)
            {
                var dialog = new InfoWindow(ShowSeriesInfo(serija, db), ShowSeriesInfo(serija, db).Split('\n').Length)
                {
                    Owner = Parent as Window
                };

                dialog.Show();
            }
        }

        public static string ShowSeriesInfo(Serija serija, FlickFolioContext db)
        {
            /*
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
            */

            return "test";

        }
    }
}
