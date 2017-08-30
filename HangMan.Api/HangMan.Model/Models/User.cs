namespace HangMan.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Player Player { get; set; }
    }
}