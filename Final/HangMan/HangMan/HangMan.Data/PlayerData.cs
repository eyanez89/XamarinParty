using HangMan.DependencyInterface;
using HangMan.Model.Model;
using Xamarin.Forms;

namespace HangMan.Data
{
    public class PlayerData : BaseData<Player>
    {
        public PlayerData() : base(DependencyService.Get<ISQLite>().GetAsyncConnection()) { }
    }
}
