using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroTecnico.Migrations
{
    /// <inheritdoc />
    public partial class tarea2prueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenicoId",
                table: "Clientes",
                newName: "TecnicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TecnicoId",
                table: "Clientes",
                newName: "TenicoId");
        }
    }
}
