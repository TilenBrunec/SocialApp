using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocialApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Objava : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string avtor = "";
        private string vsebina = "";
        private string datum = "";
        private int likes = 0;
        private string slika = "";
        private string kategorija = "";

        public string Avtor
        {
            get { return avtor; }
            set { avtor = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Avtor")); }
        }
        public string Vsebina
        {
            get { return vsebina; }
            set { vsebina = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vsebina")); }
        }
        public string Datum
        {
            get { return datum; }
            set { datum = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Datum")); }
        }
        public int Likes
        {
            get { return likes; }
            set
            {
                likes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Likes"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BrezVseckov"));
            }
        }
        public string Slika
        {
            get { return slika; }
            set { slika = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Slika")); }
        }
        public string Kategorija
        {
            get { return kategorija; }
            set { kategorija = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kategorija")); }
        }

        public bool BrezVseckov
        {
            get { return likes == 0; }
        }
    }


    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
           InitializeComponent();
           this.DataContext = new ViewModel();
        }

        private void MenuIzhodClick(object sender, RoutedEventArgs e)
              => Application.Current.Shutdown();

        private void ListViewObjave_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (ListViewObjave.SelectedItem is Objava objava) {
                MessageBox.Show(objava.Vsebina, "to je vsebina");
            }
        }

    }
}