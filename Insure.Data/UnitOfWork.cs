using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Insure.Core;
using Insure.Core.Repositories;
using Insure.Data.Repositories;

namespace Insure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InsureDbContext _context;
        private CategoryRepository _categoryRepository;
        private ItemRepository _itemRepository;

        public UnitOfWork(InsureDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository Category => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IItemRepository Item => _itemRepository = _itemRepository ?? new ItemRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
