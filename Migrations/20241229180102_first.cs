using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drugs",
                columns: table => new
                {
                    DrugID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NDC = table.Column<string>(type: "varchar(max)", nullable: false),
                    Form = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AcquisitionCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AWP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dispensed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PreviousUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DrugClass = table.Column<string>(type: "varchar(max)", nullable: false),
                    EPCClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RxCUI = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drugs", x => x.DrugID);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    ScriptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Script = table.Column<int>(type: "int", nullable: false),
                    RNumber = table.Column<int>(type: "int", nullable: false),
                    RA = table.Column<int>(type: "int", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Ins = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    PF = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Prescriber = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Qty = table.Column<double>(type: "float", nullable: false),
                    ACQ = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    InsPay = table.Column<double>(type: "float", nullable: false),
                    PatPay = table.Column<double>(type: "float", nullable: false),
                    NDC = table.Column<long>(type: "bigint", nullable: false),
                    RxCui = table.Column<double>(type: "float", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Net = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.ScriptId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drugs");

            migrationBuilder.DropTable(
                name: "Prescriptions");
        }
    }
}
