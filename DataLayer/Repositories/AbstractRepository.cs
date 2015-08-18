using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public abstract class AbstractRepository<TModel, TData> : ICrudRepository<TModel, TData>
                                where TModel : SimpleModel
                                where TData : class
    {
        public IEnumerable<TModel> GetAll()
        {
            return Context.Set<TData>().Select(FromDataToModelConverter).ToList();
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {           
            var result = await Context.Set<TData>().ToListAsync();
            return result.Select(FromDataToModelConverter);
        }

        public IEnumerable<TModel> Get(Expression<Func<TData, bool>> predicate)
        {            
            return Context.Set<TData>().Where(predicate).AsEnumerable().Select(FromDataToModelConverter).ToList();
        }

        public async Task<IEnumerable<TModel>> GetAsync(Expression<Func<TData, bool>> predicate)
        {
            var res = await Context.Set<TData>().Where(predicate).ToListAsync();
            return res.AsEnumerable().Select(FromDataToModelConverter);
        }

        public TModel Get(int id)
        {
            var res = Context.Set<TData>().Find();
            return FromDataToModelConverter(res);
        }

        public async Task<TModel> GetAsync(int id)
        {
            var res = await Context.Set<TData>().FindAsync();
            return FromDataToModelConverter(res);
        }

        public void Add(TModel item)
        {
            var toAdd = FromModelToDataConverter(item);
            Context.Set<TData>().Add(toAdd);
            Context.SaveChanges();
        }

        public async Task AddAsync(TModel item)
        {
            var toAdd = FromModelToDataConverter(item);
            Context.Set<TData>().Add(toAdd);
            await Context.SaveChangesAsync();
        }

        public void Update(TModel item)
        {
            var toUpdate = FromModelToDataConverter(item);
            Context.Set<TData>().Attach(toUpdate);
            Context.Entry(toUpdate).State = EntityState.Modified;

            Context.SaveChanges();
        }

        public async Task UpdateAsync(TModel item)
        {
            var toUpdate = FromModelToDataConverter(item);
            Context.Set<TData>().Attach(toUpdate);
            Context.Entry(toUpdate).State = EntityState.Modified;

            await Context.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var toDel = Context.Set<TData>().Find(id);
            Context.Set<TData>().Remove(toDel);
            Context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var toDel = Context.Set<TData>().Find(id);
            Context.Set<TData>().Remove(toDel);
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        protected readonly SolarSystemEntities Context = new SolarSystemEntities();

        protected abstract TData FromModelToDataConverter(TModel model);

        protected abstract TModel FromDataToModelConverter(TData data);
    }
}
