using HangMan.Data.EFContext;
using HangMan.Models;

namespace HangMan.Service
{
    public class PlayerService : EntityService<Player>, IPlayerService
    {
        public PlayerService(HangManContext db) : base(db)
        {
        }
    }
}
