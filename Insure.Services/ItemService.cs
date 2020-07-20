using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Insure.Core;
using Insure.Core.Models;
using Insure.Core.Services;

namespace Insure.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Item> CreateItem(Item item)
        {
            await unitOfWork.Item.AddAsync(item);
            await unitOfWork.CommitAsync();

            return item;
        }
    }
}
