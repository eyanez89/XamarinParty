using System.Threading.Tasks;
using HangMan.Data.EFContext;
using HangMan.Models;
using System.Linq;
using System.Data.Entity;
using System;

namespace HangMan.Service
{
    public class GameService : EntityService<Game>, IGameService
    {
        public GameService(HangManContext db) : base(db)
        {
        }

        public async Task<Game> NewGame(WordDifficulty wordDifficulty, string userName)
        {
            var user = await db.Set<User>().Include(u => u.Player).FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return null;

            var words = await db.Set<Word>().Where(w => w.Difficulty == wordDifficulty && w.CreatedBy != userName).ToListAsync();
            var rand = new Random();
            var word = words[rand.Next(words.Count)];

            if (word == null)
                return null;

            var game = new Game()
            {
                Player = user.Player,
                Word = word,
            };

            var serverGame = AttachEntity(game, state: EntityState.Added);

            await db.SaveChangesAsync();

            return serverGame;
        }

        public async Task Win(Game game)
        {
            var serverWord = await db.Set<Word>().FirstOrDefaultAsync(p => p.Id == game.Word.Id);
            var serverPlayer = await db.Set<Player>().FirstOrDefaultAsync(p => p.Id == game.Player.Id);

            var score = serverWord.Letters * (int)serverWord.Difficulty - game.Attempts;
            serverPlayer.Score += score;

            game.Word = serverWord;
            game.Player = serverPlayer;
            game.Win = true;

            AttachEntity(game);

            await db.SaveChangesAsync();
        }

        public async Task Loose(Game game)
        {
            var serverWord = await db.Set<Word>().FirstOrDefaultAsync(p => p.Id == game.Word.Id);
            var serverPlayer = await db.Set<Player>().FirstOrDefaultAsync(p => p.Id == game.Player.Id);

            var score = (int)serverWord.Difficulty * 6;
            serverPlayer.Score -= score;

            game.Word = serverWord;
            game.Player = serverPlayer;
            game.Win = false;

            AttachEntity(game);

            await db.SaveChangesAsync();
        }
    }
}
