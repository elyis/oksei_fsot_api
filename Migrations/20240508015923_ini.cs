using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_CriterionEvaluationOptions_EvaluationId",
                table: "Marks");

            migrationBuilder.RenameColumn(
                name: "EvaluationId",
                table: "Marks",
                newName: "EvaluationOptionId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Marks",
                newName: "EvaluatedAppraiserId");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_EvaluationId",
                table: "Marks",
                newName: "IX_Marks_EvaluationOptionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Marks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_CriterionEvaluationOptions_EvaluationOptionId",
                table: "Marks",
                column: "EvaluationOptionId",
                principalTable: "CriterionEvaluationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_CriterionEvaluationOptions_EvaluationOptionId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Marks");

            migrationBuilder.RenameColumn(
                name: "EvaluationOptionId",
                table: "Marks",
                newName: "EvaluationId");

            migrationBuilder.RenameColumn(
                name: "EvaluatedAppraiserId",
                table: "Marks",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_EvaluationOptionId",
                table: "Marks",
                newName: "IX_Marks_EvaluationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_CriterionEvaluationOptions_EvaluationId",
                table: "Marks",
                column: "EvaluationId",
                principalTable: "CriterionEvaluationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
