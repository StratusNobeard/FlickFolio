using FlickFolio.Models;
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

            RefreshGrid();

            CheckEmpty();
        }

        public Film? Model { get; set; }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FlickFolioContext();

            Model ??= new Film();
            
            Model.Naziv = txtName.Text;
            Model.Trajanje = txtLenght.Text;
            Model.Godina = txtYear.Text;
            Model.Trajanje = txtLenght.Text;

            var selectedDirector = cmbDirectors.SelectedItem as Redatelj;

            Model.RedateljId = selectedDirector.Id;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model != null)
            {
                txtId.Text = Model.Id.ToString();
                txtName.Text = Model.Naziv;
                txtLenght.Text = Model.Trajanje;
                txtYear.Text = Model.Godina;
                //treba još dodati
            }
        }

        private void cmbDirectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckEmpty();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmpty();
        }

        private void txtLenght_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmpty();
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmpty();
        }

        private void lbActors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckEmpty();
        }

        private void lbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckEmpty();
        }

        private void RefreshGrid()
        {
            using var db = new FlickFolioContext();
            cmbDirectors.ItemsSource = db.Redatelji.ToList();
            cmbDirectors.SelectedValuePath = "Id";
            lbActors.ItemsSource = db.Glumci.ToList();
            lbGenres.ItemsSource = db.Zanrovi.ToList();
        }

        private void CheckEmpty()
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtLenght.Text) || string.IsNullOrEmpty(txtYear.Text) || cmbDirectors.SelectedItem == null || lbActors.SelectedIndex == -1 || lbGenres.SelectedIndex == -1) btnSave.IsEnabled = false;
            else btnSave.IsEnabled = true;
        }
    }
}
