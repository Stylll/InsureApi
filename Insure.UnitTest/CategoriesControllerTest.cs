using AutoMapper;
using Insure.Api.Controllers;
using Insure.Api.Resources;
using Insure.Api.Responses;
using Insure.Core.Models;
using Insure.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insure.UnitTest
{
    public class CategoriesControllerTest
    {
        private readonly Mock<IItemService> itemService;
        private readonly Mock<ICategoryService> categoryService;
        private readonly Mock<IMapper> mapper;

        public CategoriesControllerTest()
        {
            itemService = new Mock<IItemService>();
            categoryService = new Mock<ICategoryService>();
            mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllWithItems_Returns_Success()
        {
            var itemA = new Item() { Id = 1, Name = "Television", Value = 189 };
            var itemB = new Item() { Id = 2, Name = "PS4", Value = 999 };
            var itemResourceA = new ItemResource() { Id = itemA.Id, Name = itemA.Name, Value = itemA.Value };
            var itemResourceB = new ItemResource() { Id = itemB.Id, Name = itemB.Name, Value = itemB.Value };
            var itemCollection = new List<Item>() { itemA, itemB };
            var itemResourceCollection = new List<ItemResource>() { itemResourceA, itemResourceB };
            var categoryItemsTotalA = new CategoryItemsTotal() { Id = 1, Items = itemCollection, Name = "Electronics", Total = 1188 };
            var categoryItemsTotalB = new CategoryItemsTotal() { Id = 2, Items = itemCollection, Name = "Appliances", Total = 1188 };
            var categoryItemsCollection = new List<CategoryItemsTotal>() { categoryItemsTotalA, categoryItemsTotalB };
            var categoryItemsResourceA = new CategoriesTotalResource()
            { Id = categoryItemsTotalA.Id, Name = categoryItemsTotalA.Name, Items = itemResourceCollection, Total = categoryItemsTotalA.Total };
            var categoryItemsResourceB = new CategoriesTotalResource()
            { Id = categoryItemsTotalB.Id, Name = categoryItemsTotalB.Name, Items = itemResourceCollection, Total = categoryItemsTotalB.Total };
            var categoryItemsResourceCollection = new List<CategoriesTotalResource>() { categoryItemsResourceA, categoryItemsResourceB };
            var itemTotalWithCategories = new ItemsTotalWithCategoriesResource()
            {
                categories = categoryItemsResourceCollection,
                total = 2376
            };


            categoryService.Setup(service => service.GetAllWithItems()).ReturnsAsync(categoryItemsCollection);
            mapper.Setup(mapper => mapper.Map<IEnumerable<CategoryItemsTotal>, IEnumerable<CategoriesTotalResource>>(categoryItemsCollection))
                .Returns(categoryItemsResourceCollection);
            itemService.Setup(service => service.GetItemsTotal()).Returns(2376);

            var controller = new CategoriesController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.GetAllWithItems();
            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<ObjectResult>(result.Result);
            var value = Assert.IsType<SuccessResponse<ItemsTotalWithCategoriesResource>>(res.Value);
            Assert.Equal(200, res.StatusCode);
            Assert.Equal("Categories retrieved successfully", value.Message);
            Assert.Equal(itemTotalWithCategories.total, value.Data.total);
            Assert.Equal(itemTotalWithCategories.categories.FirstOrDefault().Name, value.Data.categories.FirstOrDefault().Name);
        }

        [Fact]
        public async Task GetAllWithItems_Returns_Handled_Exception()
        {
            categoryService.Setup(service => service.GetAllWithItems()).Throws(new IOException());

            var controller = new CategoriesController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.GetAllWithItems();

            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<ObjectResult>(result.Result);
            var value = Assert.IsType<Response>(res.Value);
            Assert.Equal("An unexpected error occurred. Please try again", value.Message);
        }
    }
}
