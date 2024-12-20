using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforSalonuProje.Migrations
{
    /// <inheritdoc />
    public partial class updateKullanıcı : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "Kullanıcılar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "Kullanıcılar",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
