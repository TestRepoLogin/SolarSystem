using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Repositories
{
    public interface ICrudRepository<TModel, TData> : IDisposable
                            where TModel : SimpleModel
                            where TData : class
    {
        IEnumerable<TModel> GetAll();

        Task<IEnumerable<TModel>> GetAllAsync();

        IEnumerable<TModel> Get(Expression<Func<TData, bool>> predicate);

        Task<IEnumerable<TModel>> GetAsync(Expression<Func<TData, bool>> predicate);

        TModel Get(int id);

        Task<TModel> GetAsync(int id);

        void Add(TModel item);

        Task AddAsync(TModel item);

        void Update(TModel item);

        Task UpdateAsync(TModel item);

        void Delete(int id);

        Task DeleteAsync(int id);

        int Count { get; }    
    }
}
