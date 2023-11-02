using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MarkLogs_MarkId",
                table: "MarkLogs");

            migrationBuilder.CreateTable(
                name: "ReportTeachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CountPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    Premium = table.Column<float>(type: "REAL", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportTeachers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarkLogs_MarkId",
                table: "MarkLogs",
                column: "MarkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportTeachers_UserId",
                table: "ReportTeachers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportTeachers");

            migrationBuilder.DropIndex(
                name: "IX_MarkLogs_MarkId",
                table: "MarkLogs");

            migrationBuilder.CreateIndex(
                name: "IX_MarkLogs_MarkId",
                table: "MarkLogs",
                column: "MarkId");
        }
    }
}
