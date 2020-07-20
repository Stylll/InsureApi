using Microsoft.EntityFrameworkCore.Migrations;

namespace Insure.Data.Migrations
{
    public partial class SeedCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Categories] ON");

            migrationBuilder.Sql("INSERT INTO Categories (Id, Name) VALUES (1, 'Electronics'), (2, 'Clothing'), (3, 'Kitchen');");

            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Categories] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories");
        }
    }
}
