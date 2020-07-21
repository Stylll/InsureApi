using Insure.Core.Models;
using Insure.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insure.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(InsureDbContext context) : base(context) { }

        public async Task<IEnumerable<CategoryItemsTotal>> GetAllWithItemsTotal()
        {
            var categoryTotals = from category in context.Categories
                        select new CategoryItemsTotal
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Items = category.Items,
                            Total = (
                                from item in category.Items
                                select item.Value
                            ).Sum()
                        };
            return await categoryTotals.ToListAsync();
        }
    }
}
