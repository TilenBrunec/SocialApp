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
    /// Interaction logic for DodajObjavo.xaml
    /// </summary>
    public partial class DodajObjavo : Window
    {
        public Objava NovaObjava { get; private set; }
        private string slikaPot = "/Slike/profile.jpg";
        public DodajObjavo()
        {
            InitializeComponent();
            NovaObjava = new Objava();
            TxtDatumPrikaz.Text = DateTime.Now.ToString("d.M.yyyy");
        }

        private void ImgSlika_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Slike (*.jpeg;*.jpg;*.png)|*.jpeg;*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                slikaPot = openFileDialog.FileName;
                ImgSlika.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(slikaPot));
            }
        }
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtVsebina.Text))
            {
                MessageBox.Show("Vsebina objave ne sme biti prazna.", "Napaka");
                return; 
            }
            if(string.IsNullOrWhiteSpace(TxtKategorija.Text))
            {
                MessageBox.Show("Kategorija ne sme biti prazna.", "Napaka");
                return;
            }

            NovaObjava.Vsebina = TxtVsebina.Text;
            NovaObjava.Kategorija = TxtKategorija.Text;
            NovaObjava.Slika = slikaPot;
            NovaObjava.Datum = DateTime.Now.ToString("d.M.yyyy");
            NovaObjava.Avtor = "Tiki Eld";
            NovaObjava.Likes = 0;

            DialogResult = true;
        }
    }
}
