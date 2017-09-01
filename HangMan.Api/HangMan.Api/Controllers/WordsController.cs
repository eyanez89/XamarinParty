using HangMan.Data.EFContext;
using HangMan.Models;
using HangMan.Service;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

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


        [ResponseType(typeof(Word))]
        public async Task<IHttpActionResult> Post([FromBody]Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            switch (word.Difficulty)
            {
                case WordDifficulty.Facil:
                    if(word.GameWord.Length > 5)
                        return BadRequest();
                    break;
                case WordDifficulty.Medio:
                    if (word.GameWord.Length < 5 || word.GameWord.Length > 8)
                        return BadRequest();
                    break;
                case WordDifficulty.Dificil:
                    if (word.GameWord.Length < 8)
                        return BadRequest();
                    break;
            }

            return Ok(await wordService.Add(word));
        }
    }
}
