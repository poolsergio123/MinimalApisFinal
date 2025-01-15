using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForNewTest.Migrations
{
    /// <inheritdoc />
    public partial class actpeliculas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActorPeliculas",
                columns: table => new
                {
                    ActorModelId = table.Column<int>(type: "int", nullable: false),
                    PeliculaModelId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Personaje = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorPeliculas", x => new { x.PeliculaModelId, x.ActorModelId });
                    table.ForeignKey(
                        name: "FK_ActorPeliculas_Actores_ActorModelId",
                        column: x => x.ActorModelId,
                        principalTable: "Actores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorPeliculas_Peliculas_PeliculaModelId",
                        column: x => x.PeliculaModelId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorPeliculas_ActorModelId",
                table: "ActorPeliculas",
                column: "ActorModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorPeliculas");
        }
    }
}
