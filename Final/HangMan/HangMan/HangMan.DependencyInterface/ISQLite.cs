using SQLite.Net;
using SQLite.Net.Async;

namespace HangMan.DependencyInterface
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
