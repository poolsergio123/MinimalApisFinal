using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForNewTest.Migrations
{
    /// <inheritdoc />
    public partial class MigrationSPListarPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE spListarPersona
                AS
                BEGIN
                    SELECT * FROM Personas
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE spListarPersona");
        }
    }
}
