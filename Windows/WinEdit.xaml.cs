using FlickFolio.Prozori;
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

namespace FlickFolio.Windows
{
    /// <summary>
    /// Interaction logic for WinEdit.xaml
    /// </summary>
    public partial class WinEdit : Window
    {
        public WinEdit()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLinkHome_Click(object sender, RoutedEventArgs e)
        {
            WinMain main = new WinMain();
            Close();
            main.Show();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            WinEdit edit = new WinEdit();
            Close();
            edit.Show();
        }

        private void btnLinkMovies_Click(object sender, RoutedEventArgs e)
        {
            WinViewMovies viewMovies = new WinViewMovies();
            Close();
            viewMovies.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
