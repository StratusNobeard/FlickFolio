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
    /// Interaction logic for ActorList.xaml
    /// </summary>
    public partial class ActorList : Page
    {
        public ActorList()
        {
            InitializeComponent();

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            using var db = new FlickFolioContext();
            lbActor.ItemsSource = db.Glumci.ToList();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ActorDetails
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
            var dialog = new ActorDetails
            {
                Owner = Parent as Window
            };

            dialog.Model = lbActor.SelectedItem as Glumac;

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

                var item = lbActor.SelectedItem as Glumac;

                db.Glumci.Remove(item);
                db.SaveChanges();

                RefreshGrid();
            }
        }

        private void lbActor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEdit.IsEnabled = lbActor.SelectedItem != null;
            btnDelete.IsEnabled = lbActor.SelectedItem != null;
        }
    }
}
