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
        public async Task<ActionResult<Response>> CreateItem([FromBody]SaveItemResource item)
        {
            try
            {
                // validate input
                var validator = new SaveItemResourceValidator();
                var validationResult = await validator.ValidateAsync(item);
                var errorReponse = new ErrorResponse<IEnumerable<ValidationResource>>();

                if (!validationResult.IsValid)
                {
                    var validationErrors = mapper.Map<IEnumerable<ValidationFailure>, IEnumerable<ValidationResource>>(validationResult.Errors);
                    errorReponse.Message = "Bad Request";
                    errorReponse.Status = StatusCodes.Status400BadRequest;
                    errorReponse.Errors = validationErrors;

                    return BadRequest(errorReponse);
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
                var itemResponse = mapper.Map<Item, ItemResource>(savedItem);
                var response = new SuccessResponse<ItemResource>()
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
