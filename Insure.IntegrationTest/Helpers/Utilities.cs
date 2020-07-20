using Insure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Insure.IntegrationTest.Helpers
{
    public class Utilities
    {
        public static void SetupDb(InsureDbContext db)
        {
            using (var transactions = db.Database.BeginTransaction())
            {
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Items] ON");
                db.Database.ExecuteSqlRaw("INSERT INTO Items (Id, Name, Value, CategoryId) VALUES (1, 'Computer', 1500, 1), " +
                    "(2, 'Shirts', 200, 2), (3, 'Microwave', 124.99, 3)");
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Items] OFF");
                transactions.Commit();
            }
        }

        public static StringContent ConvertToStringContent(object bodyObject)
        {
            return new StringContent(JsonConvert.SerializeObject(bodyObject), Encoding.Default, "application/json");
        }
    }
}
