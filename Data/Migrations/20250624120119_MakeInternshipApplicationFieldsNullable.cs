using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeInternshipApplicationFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipApplications_InternshipPlaces_InternshipPlaceId",
                table: "InternshipApplications");

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "InternshipApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "InternshipPlaceId",
                table: "InternshipApplications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipApplications_InternshipPlaces_InternshipPlaceId",
                table: "InternshipApplications",
                column: "InternshipPlaceId",
                principalTable: "InternshipPlaces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipApplications_InternshipPlaces_InternshipPlaceId",
                table: "InternshipApplications");

            migrationBuilder.AlterColumn<string>(
                name: "RejectionReason",
                table: "InternshipApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InternshipPlaceId",
                table: "InternshipApplications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipApplications_InternshipPlaces_InternshipPlaceId",
                table: "InternshipApplications",
                column: "InternshipPlaceId",
                principalTable: "InternshipPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
