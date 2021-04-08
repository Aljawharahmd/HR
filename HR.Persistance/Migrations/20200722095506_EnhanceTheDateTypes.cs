using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HR.Persistance.Migrations
{
    public partial class EnhanceTheDateTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "EmployeeLeaves",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "EmployeeLeaves",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "EmployeeLeaves",
                type: "datetime",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "EmployeeLeaves",
                type: "datetime",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
