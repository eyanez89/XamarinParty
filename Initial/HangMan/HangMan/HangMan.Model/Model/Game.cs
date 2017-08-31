namespace HangMan.Model.Model
{
    public class Game : IEntity
    {
        public int Id { get; set; }

        public Player Player { get; set; }

        public Word Word { get; set; }

        public bool Win { get; set; }

        public int Attempts { get; set; }
    }
}
