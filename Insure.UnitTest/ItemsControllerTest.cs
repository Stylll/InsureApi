using AutoMapper;
using FluentValidation.Results;
using Insure.Api.Controllers;
using Insure.Api.Resources;
using Insure.Api.Responses;
using Insure.Core.Models;
using Insure.Core.Services;
using Insure.Services;
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
    public class ItemsControllerTest
    {
        private readonly Mock<IItemService> itemService;
        private readonly Mock<ICategoryService> categoryService;
        private readonly Mock<IMapper> mapper;

        public ItemsControllerTest()
        {
            itemService = new Mock<IItemService>();
            categoryService = new Mock<ICategoryService>();
            mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task CreateItem_Returns_Badrequest()
        {
            var saveItem = new SaveItemResource() { };

            var validationErrors = new List<ValidationResource>()
            {
                new ValidationResource { ErrorMessage = "Name cannot be empty", PropertyName = "Name" },
                new ValidationResource { ErrorMessage = "Value cannot be empty", PropertyName = "Value" },
                new ValidationResource { ErrorMessage = "CategoryId cannot be empty", PropertyName = "CategoryId" },
                new ValidationResource { ErrorMessage = "Value must be greater than zero", PropertyName = "Value" },
            };

            mapper.Setup(mapper =>
            mapper.Map<IEnumerable<ValidationFailure>,
            IEnumerable<ValidationResource>>(It.IsAny<IList<ValidationFailure>>()))
                .Returns(validationErrors);

            var controller = new ItemsController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.CreateItem(saveItem);

            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<BadRequestObjectResult>(result.Result);
            var value = Assert.IsType<ErrorResponse<IEnumerable<ValidationResource>>>(res.Value);
            Assert.Equal("Bad Request", value.Message);
            Assert.Equal(4, value.Errors.Count());
        }

        // Notfound
        [Fact]
        public async Task CreateItem_Returns_Notfound()
        {
            categoryService.Setup(service => service.GetCategoryById(2)).ReturnsAsync((Category)null);
            var saveItem = new SaveItemResource()
            {
                Name = "Book",
                CategoryId = 2,
                Value = 20,
            };

            var controller = new ItemsController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.CreateItem(saveItem);
            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<NotFoundObjectResult>(result.Result);
            var value = Assert.IsType<Response>(res.Value);
            Assert.Equal("Category with id: 2 does not exist", value.Message);
        }

        // CreateItem
        [Fact]
        public async Task CreateItem_Returns_Success()
        {
            var saveItem = new SaveItemResource()
            {
                Name = "Book",
                CategoryId = 2,
                Value = 20,
            };
            var saveItemModel = new Item()
            { Name = saveItem.Name, Value = saveItem.Value, CategoryId = saveItem.CategoryId };
            var category = new Category() { Id = 2, Name = "Electronics" };
            var item = new Item()
            { Id = 1, Name = saveItem.Name, Value = saveItem.Value, CategoryId = saveItem.CategoryId };
            var itemResource = new ItemResource()
            {
                Id = item.Id,
                Name = item.Name,
                Value = item.Value,
                CategoryId = item.CategoryId,
                Category = new CategoryResource() { Id = category.Id, Name = category.Name },
            };
            categoryService.Setup(service => service.GetCategoryById(2)).ReturnsAsync(category);
            itemService.Setup(service => service.CreateItem(It.IsAny<Item>())).ReturnsAsync(item);
            mapper.Setup(mapper => mapper.Map<SaveItemResource, Item>(saveItem)).Returns(item);
            mapper.Setup(mapper => mapper.Map<Item, ItemResource>(item)).Returns(itemResource);

            var controller = new ItemsController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.CreateItem(saveItem);
            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<ObjectResult>(result.Result);
            var value = Assert.IsType<SuccessResponse<ItemResource>>(res.Value);
            Assert.Equal(201, res.StatusCode);
            Assert.Equal("Item saved successfully", value.Message);
            Assert.Equal(itemResource, value.Data);
        }

        // throw exception
        [Fact]
        public async Task CreateItem_Returns_Handled_Exception()
        {
            categoryService.Setup(service => service.GetCategoryById(2)).Throws(new IOException());
            var saveItem = new SaveItemResource()
            {
                Name = "Book",
                CategoryId = 2,
                Value = 20,
            };

            var controller = new ItemsController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.CreateItem(saveItem);
            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<ObjectResult>(result.Result);
            var value = Assert.IsType<Response>(res.Value);
            Assert.Equal("An unexpected error occurred. Please try again", value.Message);
        }

        [Fact]
        public async Task DeleteItem_Returns_Notfound()
        {
            var itemId = 1;
            itemService.Setup(service => service.GetItemById(itemId)).ReturnsAsync((Item)null);
            var controller = new ItemsController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.DeleteItem(itemId);

            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<NotFoundObjectResult>(result.Result);
            var value = Assert.IsType<Response>(res.Value);
            Assert.Equal("Item with id: 1 does not exist", value.Message);
        }

        [Fact]
        public async Task DeleteItem_Returns_Ok()
        {
            var item = new Item()
            {
                Id = 1,
                Name = "Electronics",
                Value = 6000
            };
            itemService.Setup(service => service.GetItemById(item.Id)).ReturnsAsync(item);
            itemService.Setup(service => service.DeleteItem(item));

            var controller = new ItemsController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.DeleteItem(item.Id);

            Assert.IsType<ActionResult<Response>>(result);
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task DeleteItem_Returns_Handled_Exception()
        {
            itemService.Setup(service => service.GetItemById(1)).Throws(new IOException());

            var controller = new ItemsController(itemService.Object, categoryService.Object, mapper.Object);

            var result = await controller.DeleteItem(1);
            Assert.IsType<ActionResult<Response>>(result);
            var res = Assert.IsType<ObjectResult>(result.Result);
            var value = Assert.IsType<Response>(res.Value);
            Assert.Equal("An unexpected error occurred. Please try again", value.Message);
        }
    }
}
