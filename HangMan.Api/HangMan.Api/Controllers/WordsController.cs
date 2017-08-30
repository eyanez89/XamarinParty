using HangMan.Data.EFContext;
using HangMan.Models;
using HangMan.Service;
using System.Threading.Tasks;
using System.Web.Http;

namespace HangMan.Api.Controllers
{
    [Authorize]
    public class WordsController : ApiController
    {
        IWordService wordService;

        public WordsController() : this(new WordService(new HangManContext()))
        {
        }

        private WordsController(IWordService wordService)
        {
            this.wordService = wordService;
        }

        public async Task<IHttpActionResult> Post([FromBody]Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await wordService.Add(word));
        }
    }
}
