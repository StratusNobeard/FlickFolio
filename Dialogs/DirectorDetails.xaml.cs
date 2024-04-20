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

namespace FlickFolio.Dialogs
{
    /// <summary>
    /// Interaction logic for DirectorDetails.xaml
    /// </summary>
    public partial class DirectorDetails : Window
    {
        public DirectorDetails()
        {
            InitializeComponent();

            CheckEmpty();
        }

        public Redatelj? Model { get; set; }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FlickFolioContext();

            Model ??= new Redatelj();

            Model.Ime = txtName.Text;
            Model.Prezime = txtSurname.Text;

            if (Model.Id == 0)
            {
                db.Redatelji.Add(Model);
            }
            else
            {
                db.Redatelji.Update(Model);
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
                txtName.Text = Model.Ime;
                txtSurname.Text = Model.Prezime;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CheckEmpty()
        {
            if (string.IsNullOrEmpty(txtSurname.Text) || string.IsNullOrEmpty(txtName.Text)) btnSave.IsEnabled = false;
            else btnSave.IsEnabled = true;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmpty();
        }

        private void txtSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEmpty();
        }
    }
}
