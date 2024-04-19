using FlickFolio.Models;
using System.Windows;

namespace FlickFolio.Dialogs;

/// <summary>
/// Interaction logic for GenreDetails.xaml
/// </summary>
public partial class GenreDetails : Window
{
    public GenreDetails()
    {
        InitializeComponent();
    }

    public Zanr? Model { get; set; }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        using var db = new FlickFolioContext();

        Model ??= new Zanr();

        Model.Naziv = txtName.Text;

        if (Model.Id == 0) 
        {
            db.Zanrovi.Add(Model);
        }
        else 
        {
            db.Zanrovi.Update(Model);
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
        }
    }
}
