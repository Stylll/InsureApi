using Insure.Core.Models;
using Insure.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insure.Data.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(InsureDbContext context) : base(context)
        {

        }

        private InsureDbContext dbContext
        {
            get
            {
                return context as InsureDbContext;
            }
        }

        public async Task<Item> GetWithCategoryByIdAsync(int id)
        {
            return await context.Items.Include(i => i.Category).SingleOrDefaultAsync(i => i.Id == id);
        }
    }
}
