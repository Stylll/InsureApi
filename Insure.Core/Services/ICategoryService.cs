using Insure.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insure.Core.Services
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryById(int id);
    }
}
