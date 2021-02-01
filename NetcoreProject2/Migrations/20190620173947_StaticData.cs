using Microsoft.EntityFrameworkCore.Migrations;

namespace NetcoreProject2.Migrations
{
    public partial class StaticData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from [Products] where id=1");
            migrationBuilder.InsertData(
              table: "Products",
              columns: new[] { "id", "name", "description", "image", "price", "shippingCost" },
              values: new object[] { 1, "Laptop", "laptop", null, 1000, 10 });

            migrationBuilder.InsertData(
              table: "Comments",
              columns: new[] { "userid", "productId", "Rating", "image", "CommentDesc" },
              values: new object[] { 1, 1, 4, null, "Excellent" });

            migrationBuilder.InsertData(
          table: "Carts",
          columns: new[] { "id", "UserId", "productid", "Quantity", "Price" },
          values: new object[] { 1, 1, 1, 1, 10 } );
            migrationBuilder.InsertData(
        table: "Carts",
        columns: new[] { "id", "UserId", "productid", "Quantity", "Price" },
        values: new object[] { 2, 1, 1, 1, 10 });

            

            migrationBuilder.Sql("delete from [Users] where id=1");
            migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "id", "email", "username", "password", "addressline1", "addressline2", "city", "state", "country" },
            values: new object[] { 1, "anonymous@gmail.com", "anonymous", "QjYdDZ+F9E8=", "asd", "asd", "asd", "asd", "asd" });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


        }
    }
}
