using Insure.Data;
using Insure.IntegrationTest.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insure.IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<InsureDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    //db.Database.EnsureCreated(); // just creates the db
                    db.Database.EnsureDeleted();
                    db.Database.Migrate(); // creates the db and runs the migration file

                    try
                    {
                        // Seed the database with test data.
                        logger.LogError("Logging stuff here =========================================");
                        Utilities.SetupDb(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
