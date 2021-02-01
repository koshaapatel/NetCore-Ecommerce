using Microsoft.EntityFrameworkCore.Migrations;

namespace NetcoreProject2.Migrations
{
    public partial class OrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderDetails",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
