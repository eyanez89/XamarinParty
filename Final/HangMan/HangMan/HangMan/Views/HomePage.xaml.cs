
using HangMan.DependencyInterface;
using HangMan.Model.Model;
using HangMan.Service.Services;
using HangMan.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HangMan.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private BaseViewModel viewModel;
        private GameService gameService;

        public HomePage()
        {
            InitializeComponent();

            viewModel = new BaseViewModel();
            BindingContext = viewModel;
        }

        private async void StartEasyGame(object sender, System.EventArgs e)
        {
            await StartGame(WordDifficulty.Facil);
        }

        private async void StartMediumGame(object sender, System.EventArgs e)
        {
            await StartGame(WordDifficulty.Medio);
        }

        private async void StartHardGame(object sender, System.EventArgs e)
        {
            await StartGame(WordDifficulty.Dificil);
        }

        private async Task StartGame(WordDifficulty wordDifficulty)
        {
            viewModel.IsBusy = true;

            try
            {
                gameService = new GameService(await new Token().GetToken());
                var game = await gameService.NewGame(wordDifficulty);

                var gameViewModel = new GameViewModel(game);

                await Navigation.PushAsync(new GamePage(gameViewModel));
            }
            catch (System.Exception)
            {
                var notificationService = DependencyService.Get<INotificationService>();
                notificationService.Notify("No se pudo iniciar el juego.");
            }
        }

        private async void AddWord(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new WordPage());
        }
    }
}