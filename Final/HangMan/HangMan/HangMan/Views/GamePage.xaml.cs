using HangMan.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HangMan.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private GameViewModel ViewModel;

        public GamePage(GameViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            BindingContext = viewModel;
        }
    }
}