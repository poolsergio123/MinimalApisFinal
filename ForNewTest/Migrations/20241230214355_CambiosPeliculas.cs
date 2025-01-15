using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForNewTest.Migrations
{
    /// <inheritdoc />
    public partial class CambiosPeliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Foto",
                table: "Peliculas",
                newName: "Poster");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Poster",
                table: "Peliculas",
                newName: "Foto");
        }
    }
}
