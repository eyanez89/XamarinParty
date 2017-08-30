using HangMan.Data.EFContext;
using HangMan.Models;

namespace HangMan.Business
{
    public class UserService : EntityService<User>, IUserService
    {
        public UserService(HangManContext db) : base(db)
        {
        }
    }
}
