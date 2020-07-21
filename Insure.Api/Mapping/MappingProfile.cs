using AutoMapper;
using FluentValidation.Results;
using Insure.Api.Resources;
using Insure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // model to resource mapping
            CreateMap<Item, ItemCategoryResource>();
            CreateMap<Item, ItemResource>();
            CreateMap<Category, CategoryResource>();
            CreateMap<CategoryItemsTotal, CategoriesTotalResource>();

            // resource to model mapping
            CreateMap<ItemCategoryResource, Item>();
            CreateMap<CategoryResource, Category>();
            CreateMap<SaveItemResource, Item>();

            // validation to resource mapping
            CreateMap<ValidationFailure, ValidationResource>();
        }
    }
}
