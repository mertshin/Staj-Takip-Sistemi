using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInternshipDiaryApprovalSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "InternshipDiaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByAdvisorId",
                table: "InternshipDiaries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "InternshipDiaries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "InternshipDiaries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipDiaries_ApprovedByAdvisorId",
                table: "InternshipDiaries",
                column: "ApprovedByAdvisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipDiaries_Advisors_ApprovedByAdvisorId",
                table: "InternshipDiaries",
                column: "ApprovedByAdvisorId",
                principalTable: "Advisors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipDiaries_Advisors_ApprovedByAdvisorId",
                table: "InternshipDiaries");

            migrationBuilder.DropIndex(
                name: "IX_InternshipDiaries_ApprovedByAdvisorId",
                table: "InternshipDiaries");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "InternshipDiaries");

            migrationBuilder.DropColumn(
                name: "ApprovedByAdvisorId",
                table: "InternshipDiaries");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "InternshipDiaries");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "InternshipDiaries");
        }
    }
}
