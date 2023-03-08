using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendAPI.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workers_DesignationId",
                table: "Workers");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DesignationId",
                table: "Workers",
                column: "DesignationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Workers_DesignationId",
                table: "Workers");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DesignationId",
                table: "Workers",
                column: "DesignationId",
                unique: true);
        }
    }
}
