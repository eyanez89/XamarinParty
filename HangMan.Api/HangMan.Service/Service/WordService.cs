using System.Data.Entity;
using System.Threading.Tasks;
using HangMan.Data.EFContext;
using HangMan.Models;
using System.Collections.Generic;
using System.Linq;

namespace HangMan.Service
{
    public class WordService : EntityService<Word>, IWordService
    {
        public WordService(HangManContext db) : base(db)
        {
        }

        public override Task<Word> Add(Word entity)
        {
            entity.Letters = 0;
            var existingChars = new List<char>();

            foreach (char c in entity.GameWord)
            {
                if (!existingChars.Any(ec => ec == c))
                {
                    entity.Letters++;
                    existingChars.Add(c);
                }
            }

            return base.Add(entity);
        }
    }
}
