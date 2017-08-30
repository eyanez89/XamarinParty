using HangMan.Data.EFContext;
using HangMan.Models;
using HangMan.Service;
using System.Linq;
using System.Web.Http;

namespace HangMan.Api.Controllers
{
    [Authorize]
    public class PlayersController : ApiController
    {
        IPlayerService playerService;

        public PlayersController() : this(new PlayerService(new HangManContext()))
        {
        }

        private PlayersController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }

        [HttpGet]
        [Route("api/Players/GetMaxScore")]
        public IQueryable<Player> GetMaxScore(int count)
        {
            return playerService.Get().OrderBy(p => p.Score).Take(count);
        }
    }
}
