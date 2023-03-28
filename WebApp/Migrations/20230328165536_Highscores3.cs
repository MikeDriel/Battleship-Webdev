using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class Highscores3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0b045f9-ad8e-4bc5-96fb-60e61ae1c5b5", "AQAAAAIAAYagAAAAEKL0DmZ9w1dnXQ72AEWSWNbZ3Q83aeqlVPq45ZdSfU/SyFcv4R7112A6C0Oe/ZkX7w==", "a6e070ac-5259-455b-88e1-74e139129f3f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "20dfe384-183c-425c-b349-9e7e6ce8150b", "AQAAAAIAAYagAAAAECJAF+SmWBu9pn/5xJ1A5t2i8+v+Q7AA9WISzbHWrUUJqCyzRcnJT5MQ1KCDGLm7+A==", "b8276e4c-65c7-48a6-8702-2ac5a17cec01" });
        }
    }
}
