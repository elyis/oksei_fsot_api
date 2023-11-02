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
                name: "CriterionModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    LowerBound = table.Column<int>(type: "INTEGER", nullable: false),
                    UpperBound = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriterionModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Fullname = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    RoleName = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    LastEvaluationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvaluatedAppraiserModel",
                columns: table => new
                {
                    EvaluatedId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppraiserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluatedAppraiserModel", x => new { x.EvaluatedId, x.AppraiserId });
                    table.ForeignKey(
                        name: "FK_EvaluatedAppraiserModel_Users_AppraiserId",
                        column: x => x.AppraiserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluatedAppraiserModel_Users_EvaluatedId",
                        column: x => x.EvaluatedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CriterionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EvaluatedAppraiserEvaluatedId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EvaluatedAppraiserAppraiserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Marks_CriterionModel_CriterionId",
                        column: x => x.CriterionId,
                        principalTable: "CriterionModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marks_EvaluatedAppraiserModel_EvaluatedAppraiserEvaluatedId_EvaluatedAppraiserAppraiserId",
                        columns: x => new { x.EvaluatedAppraiserEvaluatedId, x.EvaluatedAppraiserAppraiserId },
                        principalTable: "EvaluatedAppraiserModel",
                        principalColumns: new[] { "EvaluatedId", "AppraiserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluatedAppraiserModel_AppraiserId",
                table: "EvaluatedAppraiserModel",
                column: "AppraiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_CriterionId",
                table: "Marks",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_EvaluatedAppraiserEvaluatedId_EvaluatedAppraiserAppraiserId",
                table: "Marks",
                columns: new[] { "EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId" });

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
                name: "Marks");

            migrationBuilder.DropTable(
                name: "CriterionModel");

            migrationBuilder.DropTable(
                name: "EvaluatedAppraiserModel");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
