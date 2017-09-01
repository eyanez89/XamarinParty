using System.Threading.Tasks;
using HangMan.Models;

namespace HangMan.Service
{
    public interface IGameService : IEntityService<Game>
    {
        Task<Game> NewGame(WordDifficulty wordDifficulty, string name);
        Task<Game> Win(Game game);
        Task<Game> Loose(Game game);
    }
}
