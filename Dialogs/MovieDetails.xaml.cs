using FlickFolio.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlickFolio.Dialogs
{
    /// <summary>
    /// Interaction logic for MovieDetails.xaml
    /// </summary>
    public partial class MovieDetails : Window
    {
        public MovieDetails()
        {
            InitializeComponent();

            InitializeDropDowns();
        }

        private Film? Model { get; set; }

        public int? MovieId { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var movie = new Film();

            if (MovieId != null)
            {
                using var db = new FlickFolioContext();

                movie = db.Filmovi
                    .Where(x => x.Id == MovieId)
                    .First();

                txtId.Text = movie.Id.ToString();
                txtName.Text = movie.Naziv.ToString();
                txtLenght.Text = movie.Trajanje.ToString();
                txtYear.Text = movie.Godina.ToString();



                //postavljanje redatelja
                var redatelj = db.Redatelji.Where(r => r.Id == movie.RedateljId).Single();
                
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
                var filmGlumci = db.FilmGlumac.Where(r => r.FilmId == movie.Id);
                var selectedIndices = new List<int>();

                foreach (Glumac glumac in lbActors.Items)
                {
                    foreach(FilmGlumac odabrano in filmGlumci)
                    {
                        if (glumac.Id == odabrano.GlumacId)
                        {
                            selectedIndices.Add(lbActors.Items.IndexOf(glumac));
                        }
                    }
                }

                foreach(int index in selectedIndices)
                {
                    lbActors.SelectedItems.Add(lbActors.Items[index]);
                }


                //postavljanje žanrova
                var filmZanrovi = db.FilmZanr.Where(r => r.FilmId == movie.Id);
                selectedIndices.Clear();

                foreach (Zanr zanr in lbGenres.Items)
                {
                    foreach (FilmZanr odabrano in filmZanrovi)
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

            }

            Model = movie;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FlickFolioContext();

            Model ??= new Film();
            
            Model.Naziv = txtName.Text;
            Model.Trajanje = int.Parse(txtLenght.Text);
            Model.Godina = int.Parse(txtYear.Text);

            var selectedDirector = cmbDirectors.SelectedItem as Redatelj;
            Model.RedateljId = selectedDirector.Id;

            var selectedActors = lbActors.SelectedItems.Cast<Glumac>().ToList();

            foreach (var actor in selectedActors)
            {
                Model.FilmGlumac.Add(new FilmGlumac { FilmId = Model.Id, GlumacId = actor.Id });
            }

            var selectedGenres = lbGenres.SelectedItems.Cast<Zanr>().ToList();

            foreach (var genre in selectedGenres)
            {
                Model.FilmZanr.Add(new FilmZanr { FilmId = Model.Id, ZanrId = genre.Id });
            }

            if (Model.Id == 0)
            {
                db.Filmovi.Add(Model);
            }
            else
            {
                db.Filmovi.Update(Model);
            }

            db.SaveChanges();

            DialogResult = true;
            Close();
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
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


        private void cmbDirectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfValid();
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

        private void lbActors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void lbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfValid();
        }

        private void InitializeDropDowns()
        {
            using var db = new FlickFolioContext();

            cmbDirectors.ItemsSource = db.Redatelji.ToList();
            //cmbDirectors.SelectedValuePath = "Id";

            lbActors.ItemsSource = db.Glumci.ToList();
            lbGenres.ItemsSource = db.Zanrovi.ToList();
        }

        private void CheckIfValid()
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtLenght.Text) || string.IsNullOrEmpty(txtYear.Text) || cmbDirectors.SelectedItem == null || lbActors.SelectedIndex == -1 || lbGenres.SelectedIndex == -1) btnSave.IsEnabled = false;
            else btnSave.IsEnabled = true;
        }
    }
}
