using Microsoft.EntityFrameworkCore.Migrations;

namespace WanoSivuv3.Migrations
{
    public partial class init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "User",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_User_ProductId",
                table: "User",
                column: "ProductId",
                unique: false);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Product_ProductId",
                table: "User",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Product_ProductId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ProductId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "User");
        }
    }
}
