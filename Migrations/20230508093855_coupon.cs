using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceWeb.Migrations
{
    public partial class coupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscountCoupon",
                columns: table => new
                {
                    CouponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valid_Till = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCoupon", x => x.CouponId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountCoupon");
        }
    }
}
