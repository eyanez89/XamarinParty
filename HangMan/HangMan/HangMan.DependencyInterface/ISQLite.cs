using SQLite.Net.Async;

namespace HangMan.DependencyInterface
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}
