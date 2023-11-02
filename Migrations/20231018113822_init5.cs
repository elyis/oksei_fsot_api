using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PremiumReportId",
                table: "ReportTeachers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PremiumReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TotalAmountPremium = table.Column<int>(type: "INTEGER", nullable: false),
                    FixedPremium = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmountPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    PartSemiannualPremium = table.Column<int>(type: "INTEGER", nullable: false),
                    DistributablePremium = table.Column<int>(type: "INTEGER", nullable: false),
                    CostByPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiumReports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportTeachers_PremiumReportId",
                table: "ReportTeachers",
                column: "PremiumReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportTeachers_PremiumReports_PremiumReportId",
                table: "ReportTeachers",
                column: "PremiumReportId",
                principalTable: "PremiumReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportTeachers_PremiumReports_PremiumReportId",
                table: "ReportTeachers");

            migrationBuilder.DropTable(
                name: "PremiumReports");

            migrationBuilder.DropIndex(
                name: "IX_ReportTeachers_PremiumReportId",
                table: "ReportTeachers");

            migrationBuilder.DropColumn(
                name: "PremiumReportId",
                table: "ReportTeachers");
        }
    }
}
