using HangMan.Model.Model;
using HangMan.Service.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HangMan.ViewModels
{
    public class ScoreViewModel : BaseViewModel
    {
        public ScoreViewModel()
        {
            Players = new ObservableCollection<Player>();
            UpdateScoreCommand = new Command(async () => { await UpdateScore(); });
            Task.Run(async () => { await UpdateScore(); });
        }

        private ObservableCollection<Player> _players;
        private ICommand _updateScoreCommand;

        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set { _players = value; OnPropertyChanged(nameof(Players)); }
        }

        public ICommand UpdateScoreCommand
        {
            get { return _updateScoreCommand; }
            set { _updateScoreCommand = value; OnPropertyChanged(nameof(UpdateScoreCommand)); }
        }

        private async Task UpdateScore()
        {
            IsBusy = true;

            try
            {
                var playerService = new PlayerService(await new Token().GetToken());
                var players = await playerService.GetMaxScoreAsync(10);

                Players.Clear();

                foreach (var player in players)
                {
                    Players.Add(player);
                }
            }
            catch (System.Exception)
            {

            }
        }
    }
}
