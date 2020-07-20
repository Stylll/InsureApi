using Insure.Core.Models;
using Insure.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insure.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(InsureDbContext context) : base(context) { }
    }
}
