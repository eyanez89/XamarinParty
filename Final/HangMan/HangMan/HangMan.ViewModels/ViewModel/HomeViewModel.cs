using HangMan.Model.Model;
using System.Windows.Input;

namespace HangMan.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ICommand _easyGame;
        private ICommand _mediumGame;
        private ICommand _hardGame;

        public ICommand EasyGame
        {
            get { return _easyGame; }
            set { _easyGame = value; OnPropertyChanged(nameof(EasyGame)); }
        }

        public ICommand MediumGame
        {
            get { return _mediumGame; }
            set { _mediumGame = value; OnPropertyChanged(nameof(MediumGame)); }
        }

        public ICommand HardGame
        {
            get { return _hardGame; }
            set { _hardGame = value; OnPropertyChanged(nameof(HardGame)); }
        }

        private void StartGame(WordDifficulty difficulty)
        {

        }
    }
}
