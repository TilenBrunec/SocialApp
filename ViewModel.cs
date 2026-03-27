using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SocialApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void Notify(string ime)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(ime));
            }
        }
        private ObservableCollection<Objava> _objave = new ObservableCollection<Objava>();
        public ObservableCollection<Objava> Objave
        {
            get { return _objave; }
            set { _objave = value; Notify("Objave"); }
        }
        private Objava? _izbranaObjava = null;
        public Objava? IzbranaObjava
        {
                        get { return _izbranaObjava; }
            set { _izbranaObjava = value; Notify("IzbranaObjava");
                  ((Command)OdstraniCommand).PosodobiCanExecute();
                  ((Command)UrediCommand).PosodobiCanExecute();
                  ((Command)VseckajCommand).PosodobiCanExecute();
            }
        }
        public ICommand DodajCommand { get; private set; }
        public ICommand UrediCommand { get;private set; }
        public ICommand OdstraniCommand { get;private set; }
        public ICommand VseckajCommand { get; private set; }

        public ViewModel()
        {
            _objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Pozdravljen svet!", Datum = "10.3.2026", Likes = 5, Slika = "/Slike/profile.jpg", Kategorija = "Splošno" });
            _objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Danes brez všečkov.", Datum = "11.3.2026", Likes = 0, Slika = "/Slike/profile.jpg", Kategorija = "Osebno" });
            _objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Programiranje je zabavno!", Datum = "12.3.2026", Likes = 12, Slika = "/Slike/profile.jpg", Kategorija = "Tehnologija" });


            DodajCommand = new Command(Dodaj);
            UrediCommand = new Command(Uredi, CanUredi);
            OdstraniCommand = new Command(Odstrani, CanOdstrani);
            VseckajCommand = new Command(Vseckaj, CanVseckaj);
        }
        private void Dodaj(object? obj)
        {
            Objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Nova objava!", Datum = DateTime.Now.ToString("d.M.yyyy"), Likes = 0, Slika = "/Slike/profile.jpg", Kategorija = "Splošno" });
        }
        private void Uredi(object? obj)
        {
            if (_izbranaObjava != null)
            {
                _izbranaObjava.Vsebina += " urejeno statocno";
                _izbranaObjava.Avtor= "Uredil dEV";
                _izbranaObjava.Likes = 999;
            }
        }
        private bool CanUredi(object? obj)
        {
            return _izbranaObjava != null;
        }
        private void Odstrani(object? obj)
        {
            if (_izbranaObjava != null)
            {
                _objave.Remove(_izbranaObjava);
            }
        }
        private bool CanOdstrani(object? obj)
        {
            return _izbranaObjava != null;
        }
        private void Vseckaj(object? obj)
        {
            if (_izbranaObjava != null)
            {
                _izbranaObjava.Likes++;
            }
        }
        private bool CanVseckaj(object? obj)
        {
            return _izbranaObjava != null;
        }

    }
}
