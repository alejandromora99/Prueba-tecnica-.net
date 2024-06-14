using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CronProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RutClienteEnvia = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    NombreClienteEnvia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdTransaccion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RutClienteRecibe = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    NombreClienteRecibe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoBancoReceptor = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    NombreBancoReceptor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MontoTransferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonedaTransferencia = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    FechaTransferencia = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataRecords");
        }
    }
}
