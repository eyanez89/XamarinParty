using SQLite.Net.Attributes;

namespace HangMan.Model
{
    public interface IEntity
    {
        [PrimaryKey]
        int Id { get; set; }
    }
}
