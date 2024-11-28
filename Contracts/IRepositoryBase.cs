using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindOne(int id);
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool UseIQueryable = false);
        Task<IQueryable<T>> FindByConditionReturnIQueryable(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        //Task<int> Save();

        // LTPE funktionalitet adderet herunder !!!
        void EnableLazyLoading();

        void DisableLazyLoading();
    }
}
