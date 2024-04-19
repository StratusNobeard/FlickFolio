using FlickFolio.Dialogs;
using FlickFolio.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlickFolio.Pages
{
    /// <summary>
    /// Interaction logic for GenreList.xaml
    /// </summary>
    public partial class GenreList : Page
    {
        public GenreList()
        {
            InitializeComponent();

            RefreshGrid();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GenreDetails
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

        private void RefreshGrid()
        {
            using var db = new FlickFolioContext();
            listBox.ItemsSource = db.Zanrovi.ToList();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = listBox.SelectedItem != null; 
            btnDelete.IsEnabled = listBox.SelectedItem != null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GenreDetails
            {
                Owner = Parent as Window
            };

            dialog.Model = listBox.SelectedItem as Zanr;

            var result = dialog.ShowDialog();
            if (result.Value)
            {
                RefreshGrid();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("Sigurni ste da želite izbrisati podatak?", "Potvrda", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes) // error is here
            {
                using var db = new FlickFolioContext();

                var item = listBox.SelectedItem as Zanr;

                db.Zanrovi.Remove(item);
                db.SaveChanges();

                RefreshGrid();
            }
        }
    }
}
