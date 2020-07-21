using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Insure.Api.Resources;
using Insure.Api.Responses;
using Insure.Core.Models;
using Insure.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Insure.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IItemService itemService;
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;
        public CategoriesController(IItemService itemService, ICategoryService categoryService, IMapper mapper)
        {
            this.itemService = itemService;
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [HttpGet("[action]")]
        [ActionName("Items")]
        public async Task<ActionResult<Response>> GetAllWithItems()
        {
            try
            {
                var categoryItems = await categoryService.GetAllWithItems();
                var categoryItemsTotalList = mapper.Map<IEnumerable<CategoryItemsTotal>, IEnumerable<CategoriesTotalResource>>(categoryItems);
                var itemsTotal = itemService.GetItemsTotal();
                var itemsTotalWithCategories = new ItemsTotalWithCategoriesResource()
                {
                    categories = categoryItemsTotalList,
                    total = itemsTotal
                };
                var response = new SuccessResponse<ItemsTotalWithCategoriesResource>()
                {
                    Data = itemsTotalWithCategories,
                    Message = "Categories retrieved successfully",
                    Status = 200
                };

                return StatusCode(200, response);
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
