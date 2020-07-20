using Insure.Api.Responses;
using InsureApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Insure.IntegrationTest.Helpers;
using Insure.Api.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Insure.IntegrationTest
{
    public class ItemsTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public ItemsTest(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Create_Endpoint_Returns_Item()
        {
            var requestBodyObject = new
            {
                Name = "Electronics",
                CategoryId = 2,
                Value = 453.23,
            };

            var request = new
            {
                url = "/api/v1/items",
                body = Utilities.ConvertToStringContent(requestBodyObject)
            };

            var client = factory.CreateClient();

            var response = await client.PostAsync(request.url, request.body);
            var value = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            var sResponse = JsonConvert.DeserializeObject<SuccessResponse<ItemResource>>(value);
            Assert.Equal("Item saved successfully", sResponse.Message);
            Assert.Equal(201, sResponse.Status);
            Assert.Equal("Electronics", sResponse.Data.Name);
        }

        [Fact]
        public async Task Delete_Endpoint_Returns_Nocontent()
        {
            var client = factory.CreateClient();
            var url = "/api/v1/items/1";

            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            Assert.Equal("NoContent", response.StatusCode.ToString());
        }
    }
}
