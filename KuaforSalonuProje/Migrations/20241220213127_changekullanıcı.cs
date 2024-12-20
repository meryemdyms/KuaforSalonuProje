using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforSalonuProje.Migrations
{
    /// <inheritdoc />
    public partial class changekullanıcı : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Kullanıcılar",
                newName: "KullanıcıAd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KullanıcıAd",
                table: "Kullanıcılar",
                newName: "Email");
        }
    }
}
