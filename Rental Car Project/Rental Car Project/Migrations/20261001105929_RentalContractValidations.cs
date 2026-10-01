using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental_Car_Project.Migrations
{
    /// <inheritdoc />
    public partial class RentalContractValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // O SQL Server não permite alterar colunas usadas por uma check constraint,
            // por isso retira-se a constraint antes e volta a criar-se no fim
            migrationBuilder.DropCheckConstraint(
                name: "CK_Rental_Dates",
                table: "RentalContracts");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "RentalContracts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "RentalContracts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Rental_Dates",
                table: "RentalContracts",
                sql: "[EndDate] > [StartDate]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Rental_Dates",
                table: "RentalContracts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "RentalContracts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "RentalContracts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Rental_Dates",
                table: "RentalContracts",
                sql: "[EndDate] > [StartDate]");
        }
    }
}