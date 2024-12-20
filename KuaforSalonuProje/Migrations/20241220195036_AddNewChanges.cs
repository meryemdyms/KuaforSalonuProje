using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforSalonuProje.Migrations
{
    /// <inheritdoc />
    public partial class AddNewChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Hizmetler");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Calisanlar");

            migrationBuilder.RenameColumn(
                name: "Resim",
                table: "Hizmetler",
                newName: "Süre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Süre",
                table: "Hizmetler",
                newName: "Resim");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Hizmetler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Calisanlar",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
