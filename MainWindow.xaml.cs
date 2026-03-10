using System.Collections.ObjectModel;
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
    public class Objava
    {
        public string Avtor { get; set; } = "";
        public string Vsebina { get; set; } = "";
        public string Datum { get; set; } = "";
        public int Likes { get; set; } = 0;

    }


    public partial class MainWindow : Window
    {
        private ObservableCollection<Objava> objave = new();
        public MainWindow()
        {
            InitializeComponent();
            ListViewObjave.ItemsSource = objave;

            objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Prva objava!", Datum = "11.3.2026", Likes = 3 });
            objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Druga objava!", Datum = "11.3.2026", Likes = 7 });
        }


        private void MenuIzhodClick(object sender, RoutedEventArgs e)
              => Application.Current.Shutdown();

        private void ListViewObjave_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ListViewObjave.SelectedItem is Objava objava
                )
            {
                MessageBox.Show(objava.Vsebina, "to je vsebina");
            }
        }
    }
}