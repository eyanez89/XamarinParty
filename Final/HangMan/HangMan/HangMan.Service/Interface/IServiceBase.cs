using HangMan.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HangMan.Service
{
    interface IServiceBase<T> where T : IEntity
    {
        Task<IEnumerable<T>> Get();
        Task<T> Get(string path);
        Task<T> Post(T entity);
    }
}
