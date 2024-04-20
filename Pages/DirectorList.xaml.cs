using FlickFolio.Dialogs;
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
    /// Interaction logic for DirectorList.xaml
    /// </summary>
    public partial class DirectorList : Page
    {
        public DirectorList()
        {
            InitializeComponent();

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            using var db = new FlickFolioContext();
            lbDirector.ItemsSource = db.Redatelji.ToList();
        }

        private void lbDirector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = lbDirector.SelectedItem != null;
            btnDelete.IsEnabled = lbDirector.SelectedItem != null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DirectorDetails
            {
                Owner = Parent as Window
            };

            dialog.Model = lbDirector.SelectedItem as Redatelj;

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

                var item = lbDirector.SelectedItem as Redatelj;

                db.Redatelji.Remove(item);
                db.SaveChanges();

                RefreshGrid();
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DirectorDetails
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
    }
}
