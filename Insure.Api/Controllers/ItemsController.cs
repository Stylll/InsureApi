using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Insure.Api.Resources;
using Insure.Api.Responses;
using Insure.Api.Validators;
using Insure.Core.Models;
using Insure.Core.Services;
using Insure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insure.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService itemService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public ItemsController(IItemService itemService, ICategoryService categoryService, IMapper mapper)
        {
            this.itemService = itemService;
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [HttpPost("")]
        public async Task<ActionResult<Response>> CreateItem([FromBody]SaveItemInputResource inputItem)
        {
            try
            {
                var errorList = new List<ErrorResource>();
                float floatValue;
                int intCategoryId;

                // custom validate input
                var isFloat = float.TryParse(inputItem.Value, out floatValue);
                var isInt = int.TryParse(inputItem.CategoryId, out intCategoryId);

                // validate name is not empty
                if (inputItem.Name == "")
                {
                    errorList.Add(new ErrorResource { PropertyName = "Name", ErrorMessage = "Name cannot be empty" });
                }

                // validate value is float
                if (!isFloat)
                {
                    errorList.Add(new ErrorResource { PropertyName = "Value", ErrorMessage = "Value must be a number or float" });
                }
                // validate categoryId is int
                if (!isInt)
                {
                    errorList.Add(new ErrorResource { PropertyName = "CategoryId", ErrorMessage = "CategoryId must be a number" });
                }

                if (errorList.Count > 0)
                {
                    var errors = new ErrorResponse<IEnumerable<ErrorResource>>();
                    errors.Message = "Bad Request";
                    errors.Status = StatusCodes.Status400BadRequest;
                    errors.Errors = errorList;

                    return BadRequest(errors);
                }

                var item = new SaveItemResource() { CategoryId = intCategoryId, Value = floatValue, Name = inputItem.Name };

                // validate input
                var validator = new SaveItemResourceValidator();
                var validationResult = await validator.ValidateAsync(item);
                var errorResponse = new ErrorResponse<IEnumerable<ValidationResource>>();

                if (!validationResult.IsValid)
                {
                    var validationErrors = mapper.Map<IEnumerable<ValidationFailure>, IEnumerable<ValidationResource>>(validationResult.Errors);
                    errorResponse.Message = "Bad Request";
                    errorResponse.Status = StatusCodes.Status400BadRequest;
                    errorResponse.Errors = validationErrors;

                    return BadRequest(errorResponse);
                }

                // check if category id already exists
                var category = await categoryService.GetCategoryById(item.CategoryId);
                if (category == null)
                {
                    var erResponse = new Response()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = string.Format("Category with id: {0} does not exist", item.CategoryId)
                    };

                    return NotFound(erResponse);
                }

                // save to db
                var itemToSave = mapper.Map<SaveItemResource, Item>(item);
                var savedItem = await itemService.CreateItem(itemToSave);
                var itemResponse = mapper.Map<Item, ItemCategoryResource>(savedItem);
                var response = new SuccessResponse<ItemCategoryResource>()
                {
                    Message = "Item saved successfully",
                    Status = StatusCodes.Status201Created,
                    Data = itemResponse
                };

                return StatusCode(StatusCodes.Status201Created, response);

            }
            catch (Exception)
            {
                var response = new Response()
                {
                    Message = "An unexpected error occurred. Please try again",
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteItem(int id)
        {
            try
            {
                var item = await itemService.GetItemById(id);
                if (item == null)
                {
                    var erResponse = new Response()
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = string.Format("Item with id: {0} does not exist", id)
                    };

                    return NotFound(erResponse);
                }

                await itemService.DeleteItem(item);

                return NoContent();
            }
            catch (Exception)
            {
                var response = new Response()
                {
                    Message = "An unexpected error occurred. Please try again",
                    Status = StatusCodes.Status500InternalServerError
                };

                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
