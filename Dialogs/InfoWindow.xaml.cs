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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlickFolio.Dialogs
{
    /// <summary>
    /// Interaction logic for InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow(string text, int lenght)
        {
            InitializeComponent();

            OutputText(text);

            ReconfigureWindow(lenght);
        }

        public string? Text { get; set; }

        public void OutputText(string text)
        {
            txtOutput.Text = text;
        }

        public void ReconfigureWindow(int lenght)
        {
            this.Height = lenght*20 + 120;
            btnClose.VerticalAlignment = VerticalAlignment.Bottom;
            btnClose.HorizontalAlignment = HorizontalAlignment.Center;
            btnClose.Margin = new Thickness(0, 0, 0, 20);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
