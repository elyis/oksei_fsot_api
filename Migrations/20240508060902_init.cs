using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Criterions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criterions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PremiumReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalAmountPremium = table.Column<int>(type: "integer", nullable: false),
                    FixedPremium = table.Column<int>(type: "integer", nullable: false),
                    TotalAmountPoints = table.Column<int>(type: "integer", nullable: false),
                    PartSemiannualPremium = table.Column<int>(type: "integer", nullable: false),
                    DistributablePremium = table.Column<int>(type: "integer", nullable: false),
                    CostByPoint = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremiumReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Fullname = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    TokenValidBefore = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastEvaluationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CriterionEvaluationOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CountPoints = table.Column<int>(type: "integer", nullable: false),
                    CriterionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriterionEvaluationOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CriterionEvaluationOptions_Criterions_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "Criterions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvaluatedAppraisers",
                columns: table => new
                {
                    EvaluatedId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppraiserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluatedAppraisers", x => new { x.EvaluatedId, x.AppraiserId });
                    table.ForeignKey(
                        name: "FK_EvaluatedAppraisers_Users_AppraiserId",
                        column: x => x.AppraiserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluatedAppraisers_Users_EvaluatedId",
                        column: x => x.EvaluatedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportTeachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CountPoints = table.Column<int>(type: "integer", nullable: false),
                    Premium = table.Column<float>(type: "real", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PremiumReportId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportTeachers_PremiumReports_PremiumReportId",
                        column: x => x.PremiumReportId,
                        principalTable: "PremiumReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportTeachers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EvaluationOptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    EvaluatedAppraiserEvaluatedId = table.Column<Guid>(type: "uuid", nullable: false),
                    EvaluatedAppraiserAppraiserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CriterionModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Marks_CriterionEvaluationOptions_EvaluationOptionId",
                        column: x => x.EvaluationOptionId,
                        principalTable: "CriterionEvaluationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marks_Criterions_CriterionModelId",
                        column: x => x.CriterionModelId,
                        principalTable: "Criterions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Marks_EvaluatedAppraisers_EvaluatedAppraiserEvaluatedId_Eva~",
                        columns: x => new { x.EvaluatedAppraiserEvaluatedId, x.EvaluatedAppraiserAppraiserId },
                        principalTable: "EvaluatedAppraisers",
                        principalColumns: new[] { "EvaluatedId", "AppraiserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    MarkId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkLogs_Marks_MarkId",
                        column: x => x.MarkId,
                        principalTable: "Marks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CriterionEvaluationOptions_CriterionId",
                table: "CriterionEvaluationOptions",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_Criterions_Name",
                table: "Criterions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluatedAppraisers_AppraiserId",
                table: "EvaluatedAppraisers",
                column: "AppraiserId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkLogs_MarkId",
                table: "MarkLogs",
                column: "MarkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_CriterionModelId",
                table: "Marks",
                column: "CriterionModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_EvaluatedAppraiserEvaluatedId_EvaluatedAppraiserAppra~",
                table: "Marks",
                columns: new[] { "EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Marks_EvaluationOptionId",
                table: "Marks",
                column: "EvaluationOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTeachers_PremiumReportId",
                table: "ReportTeachers",
                column: "PremiumReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTeachers_UserId",
                table: "ReportTeachers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Token",
                table: "Users",
                column: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkLogs");

            migrationBuilder.DropTable(
                name: "ReportTeachers");

            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "PremiumReports");

            migrationBuilder.DropTable(
                name: "CriterionEvaluationOptions");

            migrationBuilder.DropTable(
                name: "EvaluatedAppraisers");

            migrationBuilder.DropTable(
                name: "Criterions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
