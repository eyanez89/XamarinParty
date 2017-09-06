using HangMan.Data;
using HangMan.Model.Model;
using HangMan.Service.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HangMan.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public Game Game { get; set; }
        private GameService gameService;

        public GameViewModel(Game Game)
        {
            Letters = new ObservableCollection<char>();
            IncorrectLetters = new ObservableCollection<char>();

            var lettersArray = Game.Word.GameWord.ToCharArray();
            foreach (var letter in lettersArray)
            {
                Letters.Add('_');
            }

            ValidateLetterCommand = new Command(async () => await ValidateLetter());
            SetImage();
        }

        private int _attempts;
        private string _word;
        private char _letter;
        private ImageSource _hangStatus;
        private ObservableCollection<char> _letters;
        private ObservableCollection<char> _incorrectLetters;
        private ICommand _validateLetterCommand;

        public int Attempts
        {
            get { return _attempts; }
            set { _attempts = value; OnPropertyChanged(nameof(Attempts)); }
        }
        public string Word
        {
            get { return _word; }
            set { _word = value; OnPropertyChanged(nameof(Word)); }
        }
        public char Letter
        {
            get { return _letter; }
            set { _letter = value; OnPropertyChanged(nameof(Letter)); }
        }
        public ImageSource HangStatus
        {
            get { return _hangStatus; }
            set { _hangStatus = value; OnPropertyChanged(nameof(HangStatus)); }
        }
        public ObservableCollection<char> Letters
        {
            get { return _letters; }
            set { _letters = value; OnPropertyChanged(nameof(Letters)); }
        }
        public ObservableCollection<char> IncorrectLetters
        {
            get { return _incorrectLetters; }
            set { _incorrectLetters = value; OnPropertyChanged(nameof(IncorrectLetters)); }
        }
        public ICommand ValidateLetterCommand
        {
            get { return _validateLetterCommand; }
            set { _validateLetterCommand = value; OnPropertyChanged(nameof(ValidateLetterCommand)); }
        }

        private async Task ValidateLetter()
        {
            Attempts++;
            var wordArray = Word.ToCharArray();
            if (wordArray.Contains(Letter))
            {
                foreach (Match m in Regex.Matches(Word, Letter.ToString()))
                    Letters[m.Index] = Letter;

                if (!Letters.Any(c => c == '_'))
                    await Win();
            }
            else
            {
                IncorrectLetters.Add(Letter);
                SetImage();

                if (IncorrectLetters.Count == 6)
                    await Loose();
            }
        }

        private void SetImage()
        {
            var errorCount = IncorrectLetters.Count;

            switch (errorCount)
            {
                case 0:
                    HangStatus = ImageSource.FromFile("Hang.png");
                    break;
                case 1:
                    HangStatus = ImageSource.FromFile("Try_1.png");
                    break;
                case 2:
                    HangStatus = ImageSource.FromFile("Try_2.png");
                    break;
                case 3:
                    HangStatus = ImageSource.FromFile("Try_3.png");
                    break;
                case 4:
                    HangStatus = ImageSource.FromFile("Try_4.png");
                    break;
                case 5:
                    HangStatus = ImageSource.FromFile("Try_5.png");
                    break;
                case 6:
                    HangStatus = ImageSource.FromFile("Try_6.png");
                    MessagingCenter.Send(this, "LooseGame");
                    break;
            }
        }

        private async Task Win()
        {
            try
            {
                Game.Attempts = Attempts;
                Game.Win = true;

                gameService = new GameService(await new Token().GetToken());
                var player = await gameService.Win(Game);

                var playerData = new PlayerData();
                await playerData.UpdateAsync(player);
            }
            catch (Exception)
            {
                MessagingCenter.Send(this, "Ha ocurrido un error.");
            }
        }

        private async Task Loose()
        {
            try
            {
                Game.Attempts = Attempts;
                Game.Win = false;

                gameService = new GameService(await new Token().GetToken());
                var player = await gameService.Loose(Game);

                var playerData = new PlayerData();
                await playerData.UpdateAsync(player);
            }
            catch (Exception)
            {
                MessagingCenter.Send(this, "Ha ocurrido un error.");
            }

        }
    }
}
