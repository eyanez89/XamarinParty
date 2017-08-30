using System.Threading.Tasks;
using HangMan.Models;

namespace HangMan.Service
{
    public interface IGameService : IEntityService<Game>
    {
        Task<Game> NewGame(WordDifficulty wordDifficulty, string name);
        Task Win(Game game);
        Task Loose(Game game);
    }
}
