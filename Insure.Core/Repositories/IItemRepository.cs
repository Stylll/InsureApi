using Insure.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insure.Core.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Item> GetWithCategoryByIdAsync(int id);

    }
}
