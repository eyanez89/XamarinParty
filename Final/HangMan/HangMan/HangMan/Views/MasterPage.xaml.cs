using HangMan.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HangMan.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public ListView ListView;

        public MasterPage()
        {
            InitializeComponent();

            Icon = "icon.png";

            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;

            Task.Run(async () => { await SetPlayerData(); });
        }

        private async Task SetPlayerData()
        {
            var playerData = new PlayerData();
            var player = await playerData.GetFirstOrDefaultAsync();

            NickName.Text = player.NickName;
            Score.Text = player.Score.ToString();
        }

        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem(typeof(HomePage)) { Id = 0, Title = "Home" },
                    new MainPageMenuItem(typeof(ScorePage)) { Id = 1, Title = "Score" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}