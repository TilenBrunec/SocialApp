using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
namespace SocialApp
{
    /// <summary>
    /// Interaction logic for UrediObjavo.xaml
    /// </summary>
    public partial class UrediObjavo : Window
    {
        private ViewModel _model;
        public UrediObjavo(ViewModel model)
        {
            InitializeComponent();
            _model = model;
            this.DataContext = model;
        }
        private void ImgSlika_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_model.IzbranaObjava == null) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Slike (*.jpeg;*.jpg;*.png)|*.jpeg;*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                _model.IzbranaObjava.Slika = openFileDialog.FileName;
            }
        }
        private void BtnZapri_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
