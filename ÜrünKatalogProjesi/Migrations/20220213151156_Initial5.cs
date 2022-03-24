using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MezuniyetProjesi.Migrations
{
    public partial class Initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                            name: "Products",
                            columns: table => new
                            {
                                Id = table.Column<int>(type: "int", nullable: false)
                                    .Annotation("SqlServer:Identity", "1, 1"),
                                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                                Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                                Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                                ProductCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                                Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                                Price = table.Column<int>(type: "int", nullable: false),
                                IsOfferable = table.Column<bool>(type: "bit", nullable: false),
                                IsSold = table.Column<bool>(type: "bit", nullable: false),
                                CategoryId = table.Column<int>(type: "int", nullable: false),
                                UserName = table.Column<string>(type: "nvarchar(max)", nullable:false),
                                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                                ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                                CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                                ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                            },
                            constraints: table =>
                            {
                                table.PrimaryKey("PK_Products", x => x.Id);
                                table.ForeignKey(
                                    name: "FK_Products_Category_CategoryId",
                                    column: x => x.CategoryId,
                                    principalTable: "Categories",
                                    principalColumn: "Id",
                                    onDelete: ReferentialAction.Restrict);
                                
                            });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
