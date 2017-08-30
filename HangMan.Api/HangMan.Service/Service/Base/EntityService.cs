using HangMan.Data.Extensions;
using HangMan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HangMan.Service
{
    public class EntityService<T> : IEntityService<T>
        where T : class, IEntity
    {
        protected DbContext db;
        protected DbSet<T> set;

        protected EntityService(DbContext db)
        {
            this.db = db;
            set = db.Set<T>();
        }

        public virtual IQueryable<T> Get()
        {
            return set;
        }

        public virtual async Task<T> Get(Guid id)
        {
            return await set.FindAsync(id);
        }

        public virtual async Task<T> Add(T entity)
        {
            return await AddOrModify(entity, EntityState.Added);
        }

        public virtual async Task<T> Modify(T entity)
        {
            return await AddOrModify(entity);
        }

        public virtual async Task<T> Remove(Guid id)
        {
            var entity = await Get(id);

            if (entity == null)
            {
                return null;
            }

            return await Remove(entity);
        }

        public virtual async Task<T> Remove(T entity)
        {
            set.Remove(entity);
            await db.SaveChangesAsync();

            return entity;
        }

        protected virtual async Task<T> AddOrModify(T entity, EntityState? state = null)
        {
            var serverEntity = AttachEntity(entity, state: state);
            await db.SaveChangesAsync();

            return serverEntity;
        }

        protected IList<T2> AttachCollection<T2>(IEnumerable<T2> entities, bool overrideValues = false, EntityState? state = null) where T2 : class, IEntity
        {
            var serverEntities = new List<T2>(entities.Count());

            foreach (var entity in entities)
            {
                serverEntities.Add(AttachEntity(entity, overrideValues, state));
            }

            return serverEntities;
        }

        protected T2 AttachEntity<T2>(T2 entity, bool overrideValues = true, EntityState? state = null) where T2 : class, IEntity
        {
            var entry = db.AttachToOrGet(entity);

            if (overrideValues && entry.Entity != entity)
            {
                entry.CurrentValues.SetValues(entity);
            }

            if (state != null)
            {
                entry.State = state.Value;
            }

            return entry.Entity;
        }
    }
}
