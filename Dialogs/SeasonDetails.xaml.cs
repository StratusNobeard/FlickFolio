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
    /// Interaction logic for SeasonDetails.xaml
    /// </summary>
    public partial class SeasonDetails : Window
    {
        public SeasonDetails(int redniBroj)
        {
            InitializeComponent();

            tbId.Text = (redniBroj + 1).ToString();
        }

        public int? Number { get; private set; }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Number = int.Parse(tbEpisodeCount.Text);
            DialogResult = true;

            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbEpisodeCounter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbEpisodeCount.Text == "")
            {
                btnSave.IsEnabled = false;
            } else
            {
                btnSave.IsEnabled = true;
            }
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

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


    }
}
