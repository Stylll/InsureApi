using Insure.Api.Resources;
using Insure.Api.Responses;
using InsureApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insure.IntegrationTest
{
    public class CategoriesTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public CategoriesTest(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetAllWithItems_Returns_Success()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/v1/categories/items");
            var value = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            var sResponse = JsonConvert.DeserializeObject<SuccessResponse<ItemsTotalWithCategoriesResource>>(value);
            Assert.Equal("Categories retrieved successfully", sResponse.Message);
            Assert.Equal(200, sResponse.Status);
            Assert.Equal(3, sResponse.Data.categories.Count());
        }
    }
}
