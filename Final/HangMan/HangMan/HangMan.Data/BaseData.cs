using HangMan.Model;
using SQLite.Net.Async;
using System.Threading.Tasks;

namespace HangMan.Data
{
    public class BaseData<T> where T : class, IEntity
    {
        private SQLiteAsyncConnection Database;

        public BaseData(SQLiteAsyncConnection conection)
        {
            Database = conection;
            Database.CreateTableAsync<T>();
        }

        public async Task<T> GetFirstOrDefaultAsync()
        {
            return await Database.Table<T>().FirstOrDefaultAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await Database.InsertAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await Database.UpdateAsync(entity);
        }

        public async Task DeleteAllAsync()
        {
            await Database.DeleteAllAsync<T>();
        }
    }
}
