using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KuaforSalonuProje.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sure",
                table: "Hizmetler");

            migrationBuilder.AlterColumn<decimal>(
                name: "Ucret",
                table: "Hizmetler",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "HizmetAdi",
                table: "Hizmetler",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Ucret",
                table: "Hizmetler",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "HizmetAdi",
                table: "Hizmetler",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Sure",
                table: "Hizmetler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
