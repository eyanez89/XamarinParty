namespace HangMan.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        private string _nickName;
        private int _score;

        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; OnPropertyChanged(nameof(NickName)); }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; OnPropertyChanged(nameof(Score)); }
        }
    }
}
