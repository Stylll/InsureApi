using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insure.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
    }
}
