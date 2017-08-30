using HangMan.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace HangMan.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static DbEntityEntry<T> AttachToOrGet<T>(this DbContext context, T entity) where T : class, IEntity
        {
            var entry = entity.Id != default(int) ? context.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity.Id == entity.Id) : null;
            if (entry == null)
            {
                context.Set<T>().Attach(entity);
                entry = context.Entry(entity);
            }

            return entry;
        }
    }
}
