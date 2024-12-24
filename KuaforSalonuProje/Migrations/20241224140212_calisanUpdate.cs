using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforSalonuProje.Migrations
{
    /// <inheritdoc />
    public partial class calisanUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusaitSaatler",
                table: "Calisanlar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MusaitSaatler",
                table: "Calisanlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
