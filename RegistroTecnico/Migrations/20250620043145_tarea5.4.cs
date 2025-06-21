using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroTecnico.Migrations
{
    /// <inheritdoc />
    public partial class tarea54 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Sistemas_SistemaId",
                table: "Ventas");

            migrationBuilder.DropForeignKey(
                name: "FK_VentasDetalles_Ventas_ventasVentaId",
                table: "VentasDetalles");

            migrationBuilder.DropIndex(
                name: "IX_VentasDetalles_ventasVentaId",
                table: "VentasDetalles");

            migrationBuilder.DropIndex(
                name: "IX_Ventas_SistemaId",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "VentasDetalles");

            migrationBuilder.DropColumn(
                name: "ventasVentaId",
                table: "VentasDetalles");

            migrationBuilder.DropColumn(
                name: "SistemaId",
                table: "Ventas");

            migrationBuilder.AddColumn<double>(
                name: "Monto",
                table: "VentasDetalles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_VentasDetalles_SistemaId",
                table: "VentasDetalles",
                column: "SistemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VentasDetalles_Sistemas_SistemaId",
                table: "VentasDetalles",
                column: "SistemaId",
                principalTable: "Sistemas",
                principalColumn: "SistemaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VentasDetalles_Sistemas_SistemaId",
                table: "VentasDetalles");

            migrationBuilder.DropIndex(
                name: "IX_VentasDetalles_SistemaId",
                table: "VentasDetalles");

            migrationBuilder.DropColumn(
                name: "Monto",
                table: "VentasDetalles");

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "VentasDetalles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ventasVentaId",
                table: "VentasDetalles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SistemaId",
                table: "Ventas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VentasDetalles_ventasVentaId",
                table: "VentasDetalles",
                column: "ventasVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_SistemaId",
                table: "Ventas",
                column: "SistemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Sistemas_SistemaId",
                table: "Ventas",
                column: "SistemaId",
                principalTable: "Sistemas",
                principalColumn: "SistemaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VentasDetalles_Ventas_ventasVentaId",
                table: "VentasDetalles",
                column: "ventasVentaId",
                principalTable: "Ventas",
                principalColumn: "VentaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
