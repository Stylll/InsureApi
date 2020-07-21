using Insure.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Insure.Core.Services
{
    public interface IItemService
    {
        Task<Item> CreateItem(Item item);
        Task<Item> GetItemById(int id);
        Task DeleteItem(Item item);
        float GetItemsTotal();
    }
}
