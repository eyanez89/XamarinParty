using System.Collections.Generic;

namespace HangMan.Models
{
    public class Player : IEntity
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public int Score { get; set; }

        public IList<Game> Games { get; set; }
    }
}