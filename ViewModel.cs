using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Recognition;
using System.Speech.Synthesis;

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
            set
            {
                _izbranaObjava = value;
                Notify("IzbranaObjava");
                ((Command)OdstraniCommand).PosodobiCanExecute();
                ((Command)UrediCommand).PosodobiCanExecute();
                ((Command)VseckajCommand).PosodobiCanExecute();
                ((Command)OdpriUrediCommand).PosodobiCanExecute();

                if (_izbranaObjava != null)
                    NaložiGrammar("selected");
                else
                    NaložiGrammar("basic");
            }
        }

        private UrediObjavo? _urediWindow = null;

        private SpeechRecognitionEngine? _recognizer;
        private SpeechSynthesizer? _synthesizer;
        private string _voiceState = "basic";

        private string _currentCommands = "";
        public string CurrentCommands
        {
            get { return _currentCommands; }
            set { _currentCommands = value; Notify("CurrentCommands"); }
        }

        public Action<int>? VoiceTabChange;

        public ICommand DodajCommand { get; private set; }
        public ICommand UrediCommand { get; private set; }
        public ICommand OdstraniCommand { get; private set; }
        public ICommand VseckajCommand { get; private set; }
        public ICommand OdpriDodajCommand { get; private set; }
        public ICommand OdpriUrediCommand { get; private set; }

        public ViewModel()
        {
            _objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Pozdravljen svet", Datum = "10.3.2026", Likes = 5, Slika = "/Slike/profile.jpg", Kategorija = "Splošno" });
            _objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Mr beast.", Datum = "11.3.2026", Likes = 0, Slika = "/Slike/profile.jpg", Kategorija = "Osebno" });
            _objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Zdarvoo ", Datum = "12.3.2026", Likes = 12, Slika = "/Slike/profile.jpg", Kategorija = "Tehnologija" });

            DodajCommand = new Command(Dodaj);
            UrediCommand = new Command(Uredi, CanUredi);
            OdstraniCommand = new Command(Odstrani, CanOdstrani);
            VseckajCommand = new Command(Vseckaj, CanVseckaj);
            OdpriDodajCommand = new Command(OdpriDodaj);
            OdpriUrediCommand = new Command(OdpriUredi, CanUredi);

            InitVoice();
        }

        private void InitVoice()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();
                _synthesizer.SetOutputToDefaultAudioDevice();

                _recognizer = new SpeechRecognitionEngine();
                _recognizer.SetInputToDefaultAudioDevice();
                _recognizer.SpeechRecognized += OnSpeechRecognized;

                NaložiGrammar("basic");
            }
            catch (Exception ex)
            {
                CurrentCommands = "Voice not available: " + ex.Message;
            }
        }

        private void NaložiGrammar(string state)
        {
            if (_recognizer == null) return;

            try { _recognizer.RecognizeAsyncStop(); } catch { }

            _recognizer.UnloadAllGrammars();
            _voiceState = state;

            Choices commands = new Choices();

            if (state == "basic")
            {
                commands.Add(new string[] { "add post", "settings", "help" });
                CurrentCommands = "Voice commands: add post | settings | help";
            }
            else if (state == "selected")
            {
                commands.Add(new string[] { "like", "edit", "delete", "cancel" });
                CurrentCommands = "Voice commands: like | edit | delete | cancel";
            }
            else if (state == "yes")
            {
                commands.Add(new string[] { "yes", "cancel" });
                CurrentCommands = "Voice commands: yes | cancel";
            }

            GrammarBuilder gb = new GrammarBuilder(commands);
            Grammar grammar = new Grammar(gb);
            _recognizer.LoadGrammar(grammar);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            string text = e.Result.Text;

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (_voiceState == "basic")
                {
                    if (text == "add post")
                    {
                        _synthesizer?.SpeakAsync("Opening add post window");
                        OdpriDodaj(null);
                    }
                    else if (text == "settings")
                    {
                        _synthesizer?.SpeakAsync("Switching to about me tab");
                        VoiceTabChange?.Invoke(1);
                    }
                    else if (text == "help")
                    {
                        _synthesizer?.SpeakAsync("Available commands are: add post, settings, help");
                    }
                }
                else if (_voiceState == "selected")
                {
                    if (text == "like")
                    {
                        Vseckaj(null);
                        _synthesizer?.SpeakAsync("Post liked!");
                    }
                    else if (text == "edit")
                    {
                        _synthesizer?.SpeakAsync("Opening edit window");
                        OdpriUredi(null);
                    }
                    else if (text == "delete")
                    {
                        _synthesizer?.SpeakAsync("Are you sure? Say yes or cancel");
                        NaložiGrammar("yes");
                    }
                    else if (text == "cancel")
                    {
                        _synthesizer?.SpeakAsync("Selection cancelled");
                        _izbranaObjava = null;
                        Notify("IzbranaObjava");
                        ((Command)OdstraniCommand).PosodobiCanExecute();
                        ((Command)UrediCommand).PosodobiCanExecute();
                        ((Command)VseckajCommand).PosodobiCanExecute();
                        ((Command)OdpriUrediCommand).PosodobiCanExecute();
                        NaložiGrammar("basic");
                    }
                }
                else if (_voiceState == "yes")
                {
                    if (text == "yes")
                    {
                        _synthesizer?.SpeakAsync("Post deleted");
                        if (_izbranaObjava != null)
                            _objave.Remove(_izbranaObjava);
                        _izbranaObjava = null;
                        Notify("IzbranaObjava");
                        ((Command)OdstraniCommand).PosodobiCanExecute();
                        ((Command)UrediCommand).PosodobiCanExecute();
                        ((Command)VseckajCommand).PosodobiCanExecute();
                        ((Command)OdpriUrediCommand).PosodobiCanExecute();
                        NaložiGrammar("basic");
                    }
                    else if (text == "cancel")
                    {
                        _synthesizer?.SpeakAsync("Deletion cancelled");
                        NaložiGrammar("selected");
                    }
                }
            });
        }

        private void Dodaj(object? obj)
        {
            Objave.Add(new Objava { Avtor = "Tiki Eld", Vsebina = "Nova objava!", Datum = DateTime.Now.ToString("d.M.yyyy"), Likes = 0, Slika = "/Slike/profile.jpg", Kategorija = "Splošno" });
        }

        private void Uredi(object? obj)
        {
            if (_izbranaObjava != null)
            {
                _izbranaObjava.Vsebina += " urejeno statično";
                _izbranaObjava.Avtor = "Uredil dEV";
                _izbranaObjava.Likes = 0;
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
                _izbranaObjava = null;
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

        private void OdpriDodaj(object? obj)
        {
            DodajObjavo dodajWindow = new DodajObjavo();
            if (dodajWindow.ShowDialog() == true)
            {
                _objave.Add(dodajWindow.NovaObjava);
            }
        }

        private void OdpriUredi(object? obj)
        {
            if (_urediWindow == null)
            {
                _urediWindow = new UrediObjavo(this);
                _urediWindow.Owner = System.Windows.Application.Current.MainWindow;
                _urediWindow.Closed += OnUrediWindowClosed;
                _urediWindow.Show();
            }
        }

        private void OnUrediWindowClosed(object? sender, EventArgs e)
        {
            _urediWindow = null;
        }
    }
}