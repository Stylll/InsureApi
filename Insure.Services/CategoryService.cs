using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Insure.Core;
using Insure.Core.Models;
using Insure.Core.Services;

namespace Insure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await unitOfWork.Category.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CategoryItemsTotal>> GetAllWithItems()
        {
            var categoryList = await unitOfWork.Category.GetAllWithItemsTotal();
            return categoryList;
        }
    }
}
