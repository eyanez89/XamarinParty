using System.Data.Entity;
using System.Threading.Tasks;
using HangMan.Data.EFContext;
using HangMan.Models;

namespace HangMan.Service
{
    public class UserService : EntityService<User>, IUserService
    {
        public UserService(HangManContext db) : base(db)
        {
        }

        protected override async Task<User> AddOrModify(User user, EntityState? state = null)
        {
            var serverPlayer = AttachEntity(user.Player);
            var serverUser = AttachEntity(user, state:state);
            serverUser.Player = serverPlayer;

            await db.SaveChangesAsync();

            return serverUser;
        }
    }
}
