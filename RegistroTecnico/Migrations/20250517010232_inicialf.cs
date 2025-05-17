using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroTecnico.Migrations
{
    /// <inheritdoc />
    public partial class inicialf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tecnicos",
                columns: table => new
                {
                    TecnicoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TecnicoNombre = table.Column<string>(type: "TEXT", nullable: false),
                    SueldoHora = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnicos", x => x.TecnicoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tecnicos");
        }
    }
}
