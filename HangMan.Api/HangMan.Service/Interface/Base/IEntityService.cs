using HangMan.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HangMan.Service
{
    public interface IEntityService<T> where T : IEntity
    {
        Task<T> Add(T entity);
        Task<T> Get(Guid id);
        IQueryable<T> Get();
        Task<T> Modify(T entity);
        Task<T> Remove(Guid id);
        Task<T> Remove(T entity);
    }
}
