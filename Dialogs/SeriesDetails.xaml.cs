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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace FlickFolio.Dialogs
{
    /// <summary>
    /// Interaction logic for SeriesDetails.xaml
    /// </summary>
    public partial class SeriesDetails : Window
    {
        public SeriesDetails(bool update)
        {
            InitializeComponent();

            InitializeDropDowns();

            CheckIfValid();

            Update = update;
        }

        public bool Update { get; set; }

        public class ListBoxItem
        {
            public int RedniBroj { get; set; }
            public int BrojEpizoda { get; set; }
        }

        private Serija? Model { get; set; }

        public int? SeriesId { get; set; }



        private void winSeriesDetails_Loaded(object sender, RoutedEventArgs e)
        {

            var series = new Serija();

            if (SeriesId != null)
            {

                using var db = new FlickFolioContext();

                series = db.Serije
                    .Where(x => x.Id == SeriesId)
                    .First();

                tbId.Text = series.Id.ToString();
                tbName.Text = series.Naziv.ToString();
                tbYearStart.Text = series.PocetnaGodina.ToString();
                tbYearEnd.Text = series.ZavrsnaGodina.ToString();



                //postavljanje redatelja
                var redatelj = db.Redatelji.Where(r => r.Id == series.RedateljId).Single();

                int selectedIndex = -1;

                foreach (Redatelj item in cmbDirectors.Items)
                {
                    if (item.Id == redatelj.Id)
                    {
                        selectedIndex = cmbDirectors.Items.IndexOf(item);
                        break;
                    }
                }

                cmbDirectors.SelectedIndex = selectedIndex;


                //postavljanje glumaca
                var serijaGlumac = db.SerijaGlumac.Where(r => r.SerijaId == series.Id);
                var selectedIndices = new List<int>();

                foreach (Glumac glumac in lbActors.Items)
                {
                    foreach (SerijaGlumac odabrano in serijaGlumac)
                    {
                        if (glumac.Id == odabrano.GlumacId)
                        {
                            selectedIndices.Add(lbActors.Items.IndexOf(glumac));
                        }
                    }
                }

                foreach (int index in selectedIndices)
                {
                    lbActors.SelectedItems.Add(lbActors.Items[index]);
                }


                //postavljanje žanrova
                var serijaZanrovi = db.SerijaZanr.Where(r => r.SerijaId == series.Id);
                selectedIndices.Clear();

                foreach (Zanr zanr in lbGenres.Items)
                {
                    foreach (SerijaZanr odabrano in serijaZanrovi)
                    {
                        if (zanr.Id == odabrano.ZanrId)
                        {
                            selectedIndices.Add(lbGenres.Items.IndexOf(zanr));
                        }
                    }
                }

                foreach (int index in selectedIndices)
                {
                    lbGenres.SelectedItems.Add(lbGenres.Items[index]);
                }


                //postavljanje listboxa
                var skup = db.Sezone
                    .Where(s => s.SerijaId == series.Id)
                    .Select(s => s.BrojEpizoda)
                    .ToList();

                foreach (int item in skup)
                {
                    var listItem = new ListBoxItem { RedniBroj = lbSeasons.Items.Count + 1, BrojEpizoda = item };
                    lbSeasons.Items.Add(listItem);
                }

                UpdateSeasonOrder();
            }

            Model = series;

        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void InitializeDropDowns()
        {
            using var db = new FlickFolioContext();

            cmbDirectors.ItemsSource = db.Redatelji.ToList();

            lbActors.ItemsSource = db.Glumci.ToList();
            lbGenres.ItemsSource = db.Zanrovi.ToList();
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            using var db = new FlickFolioContext();

            Model ??= new Serija();

            

            Model.Naziv = tbName.Text;
            Model.BrojSezona = lbSeasons.Items.Count;
            Model.PocetnaGodina = int.Parse(tbYearStart.Text);
            Model.ZavrsnaGodina = int.Parse(tbYearEnd.Text);


            var selectedDirector = cmbDirectors.SelectedItem as Redatelj;
            Model.RedateljId = selectedDirector.Id;

            //sezone

            

            var existingActors = db.SerijaGlumac.Where(sg => sg.SerijaId == Model.Id).ToList();
            db.SerijaGlumac.RemoveRange(existingActors);

            var existingGenres = db.SerijaZanr.Where(sz => sz.SerijaId == Model.Id).ToList();
            db.SerijaZanr.RemoveRange(existingGenres);

            var selectedActorIds = lbActors.SelectedItems.Cast<Glumac>().Select(a => a.Id).ToList();
            foreach (var actorId in selectedActorIds)
            {
                if (!existingActors.Any(sg => sg.GlumacId == actorId))
                {
                    Model.SerijaGlumac.Add(new SerijaGlumac { SerijaId = Model.Id, GlumacId = actorId });
                }
            }

            var selectedGenreIds = lbGenres.SelectedItems.Cast<Zanr>().Select(g => g.Id).ToList();
            foreach (var genreId in selectedGenreIds)
            {
                if (!existingGenres.Any(sz => sz.ZanrId == genreId))
                {
                    Model.SerijaZanr.Add(new SerijaZanr { SerijaId = Model.Id, ZanrId = genreId });
                }
            }

            if (Model.Id == 0)
            {
                db.Serije.Add(Model);
            }
            else
            {
                db.Serije.Update(Model);
            }

            db.SaveChanges();



            var connection = db.Connection;
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "PRAGMA foreign_keys = 0;";
            command.ExecuteNonQuery();


            var existingSeasons = db.Sezone.Where(s => s.SerijaId == Model.Id).ToList();
            if (existingSeasons.Count != 0)
            {
                db.Sezone.RemoveRange(existingSeasons);
            }

            foreach (ListBoxItem listBoxItem in lbSeasons.Items)
            {
                var sezona = new Sezona
                {
                    RedniBroj = listBoxItem.RedniBroj,
                    BrojEpizoda = listBoxItem.BrojEpizoda,
                    SerijaId = Model.Id
                };

                db.Sezone.Add(sezona);
            }

            db.SaveChanges();


            DialogResult = true;
            Close();

        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }






        private void btnNewSeason_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SeasonDetails(lbSeasons.Items.Count);
            if (dialog.ShowDialog() == true)
            {
                int? number = dialog.Number;

                int numberConvert = number.HasValue ? number.Value : 0;

                var item = new ListBoxItem { RedniBroj = lbSeasons.Items.Count + 1, BrojEpizoda = numberConvert};

                lbSeasons.Items.Add(item);
            }

            CheckIfValid();
            UpdateSeasonOrder();
        }

        private void btnEditSeason_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SeasonDetails(lbSeasons.SelectedIndex);
            if (dialog.ShowDialog() == true)
            {
                int? number = dialog.Number;
                int numberConvert = number.HasValue ? number.Value : 0;

                var item = new ListBoxItem { RedniBroj = lbSeasons.SelectedIndex, BrojEpizoda = numberConvert };
                lbSeasons.Items[lbSeasons.SelectedIndex] = item;
            }

            CheckIfValid();
            UpdateSeasonOrder();
        }

        private void btnDeleteSeason_Click(object sender, RoutedEventArgs e)
        {

            var dialogResult = MessageBox.Show("Sigurni ste da želite izbrisati sezonu?", "Potvrda", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                lbSeasons.Items.Remove(lbSeasons.SelectedItem);
            }

            CheckIfValid();
            UpdateSeasonOrder();
        }



        private void lbSeasons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditSeason.IsEnabled = lbSeasons.SelectedItem != null;
            btnDeleteSeason.IsEnabled = lbSeasons.SelectedItem != null;
            btnDown.IsEnabled = lbSeasons.SelectedItem != null;
            btnUp.IsEnabled = lbSeasons.SelectedItem != null;
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

        private void CheckIfValid()
        {
            if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbYearEnd.Text) || string.IsNullOrEmpty(tbYearEnd.Text) || cmbDirectors.SelectedItem == null || lbActors.SelectedIndex == -1 || lbGenres.SelectedIndex == -1 || lbSeasons.Items.Count == 0  || int.Parse(tbYearStart.Text) > int.Parse(tbYearEnd.Text)) btnSave.IsEnabled = false;
            else btnSave.IsEnabled = true;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void txtLenght_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void cmbDirectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void lbActors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void lbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selected = lbSeasons.SelectedItem as ListBoxItem;
            if (selected != null)
            {
                int index = lbSeasons.SelectedIndex;
                if (index > 0)
                {
                    lbSeasons.Items.RemoveAt(index);
                    lbSeasons.Items.Insert(index - 1, selected);
                    lbSeasons.SelectedIndex = index - 1;
                }
                CheckIfValid();
            }

            UpdateSeasonOrder();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selected = lbSeasons.SelectedItem as ListBoxItem;
            if (selected != null)
            {
                int index = lbSeasons.SelectedIndex;
                if (index < lbSeasons.Items.Count - 1)
                {
                    lbSeasons.Items.RemoveAt(index);
                    lbSeasons.Items.Insert(index + 1, selected);
                    lbSeasons.SelectedIndex = index + 1;
                }
                CheckIfValid();
            }

            UpdateSeasonOrder();
        }

        private void UpdateSeasonOrder()
        {
            int redniBroj = 1;

            foreach (var item in lbSeasons.Items)
            {
                if (item is ListBoxItem listBoxItem)
                {
                    listBoxItem.RedniBroj = redniBroj++;
                }
            }

            lbSeasons.Items.Refresh();
        }
    }
}
