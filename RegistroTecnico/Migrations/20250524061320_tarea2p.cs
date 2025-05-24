using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroTecnico.Migrations
{
    /// <inheritdoc />
    public partial class tarea2p : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Tecnicos_TecnicosTecnicoId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_TecnicosTecnicoId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "TecnicosTecnicoId",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Tecnicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TecnicoId",
                table: "Clientes",
                column: "TecnicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Tecnicos_TecnicoId",
                table: "Clientes",
                column: "TecnicoId",
                principalTable: "Tecnicos",
                principalColumn: "TecnicoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Tecnicos_TecnicoId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_TecnicoId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Tecnicos");

            migrationBuilder.AddColumn<int>(
                name: "TecnicosTecnicoId",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TecnicosTecnicoId",
                table: "Clientes",
                column: "TecnicosTecnicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Tecnicos_TecnicosTecnicoId",
                table: "Clientes",
                column: "TecnicosTecnicoId",
                principalTable: "Tecnicos",
                principalColumn: "TecnicoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
