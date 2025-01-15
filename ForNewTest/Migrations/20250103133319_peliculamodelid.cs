using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForNewTest.Migrations
{
    /// <inheritdoc />
    public partial class peliculamodelid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaModelId",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "PeliculaId",
                table: "Comentarios");

            migrationBuilder.AlterColumn<int>(
                name: "PeliculaModelId",
                table: "Comentarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaModelId",
                table: "Comentarios",
                column: "PeliculaModelId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaModelId",
                table: "Comentarios");

            migrationBuilder.AlterColumn<int>(
                name: "PeliculaModelId",
                table: "Comentarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PeliculaId",
                table: "Comentarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaModelId",
                table: "Comentarios",
                column: "PeliculaModelId",
                principalTable: "Peliculas",
                principalColumn: "Id");
        }
    }
}
