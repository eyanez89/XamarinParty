namespace HangMan.Models
{
    public class Word : IEntity
    {
        public int Id { get; set; }

        public string GameWord { get; set; }

        public int Letters { get; set; }

        public WordDifficulty Difficulty { get; set; }

        public string CreatedBy { get; set; }
    }
}