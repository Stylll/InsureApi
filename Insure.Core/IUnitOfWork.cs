using Insure.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insure.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IItemRepository Item { get; }
        Task<int> CommitAsync();
    }
}
