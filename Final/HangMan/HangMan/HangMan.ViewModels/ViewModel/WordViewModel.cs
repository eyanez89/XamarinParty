using System;
using System.Collections.Generic;
using System.Windows.Input;
using HangMan.Model.Model;
using HangMan.Service.Services;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace HangMan.ViewModels
{
    public class WordViewModel : BaseViewModel
    {
        public WordViewModel()
        {
            AddWordCommand = new Command(async () => await AddWord());
        }

        private string _gameWord;
        private int _difficultySelectedIndex;
        private List<WordDifficulty> _difficulties = new List<WordDifficulty>
        {
            WordDifficulty.Facil,
            WordDifficulty.Medio,
            WordDifficulty.Dificil,
        };
        private WordDifficulty _difficulty;
        private ICommand _addWordCommand;


        public string GameWord
        {
            get { return _gameWord; }
            set { _gameWord = value; OnPropertyChanged(nameof(GameWord)); }
        }
        public List<WordDifficulty> Difficulties => _difficulties;
        public WordDifficulty Difficulty
        {
            get { return _difficulty; }
            set { _difficulty = value; OnPropertyChanged(nameof(Difficulty)); }
        }
        public int DifficultySelectedIndex
        {
            get { return _difficultySelectedIndex; }
            set
            {
                if (_difficultySelectedIndex != value)
                {
                    _difficultySelectedIndex = value;

                    // trigger some action to take such as updating other labels or fields
                    OnPropertyChanged(nameof(_difficultySelectedIndex));
                    Difficulty = Difficulties[_difficultySelectedIndex];
                }
            }
        }
        public ICommand AddWordCommand
        {
            get { return _addWordCommand; }
            set { _addWordCommand = value; OnPropertyChanged(nameof(AddWordCommand)); }
        }

        private async Task AddWord()
        {
            IsBusy = true;

            try
            {
                var word = new Word()
                {
                    GameWord = GameWord,
                    Difficulty = Difficulty
                };

                var wordService = new WordService(await new Token().GetToken());
                await wordService.Post(word);

                MessagingCenter.Send(this, "Added");
            }
            catch (Exception)
            {
                MessagingCenter.Send(this, "Exception");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
