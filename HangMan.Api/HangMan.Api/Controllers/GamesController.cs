using HangMan.Data.EFContext;
using HangMan.Models;
using HangMan.Service;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace HangMan.Api.Controllers
{
    [Authorize]
    public class GamesController : ApiController
    {
        IGameService gameService;

        public GamesController() : this(new GameService(new HangManContext()))
        {
        }

        private GamesController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpPost]
        [Route("api/Games/NewGame")]
        [ResponseType(typeof(Game))]
        public async Task<IHttpActionResult> NewGame(WordDifficulty wordDifficulty)
        {
            return Ok(await gameService.NewGame(wordDifficulty, HttpContext.Current.User.Identity.Name));
        }

        [HttpPost]
        [Route("api/Games/Win")]
        public async Task<IHttpActionResult> Win(Game game)
        {
            await gameService.Win(game);

            return Ok();
        }

        [HttpPost]
        [Route("api/Games/Loose")]
        public async Task<IHttpActionResult> Loose(Game game)
        {
            await gameService.Loose(game);

            return Ok();
        }
    }
}
