using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Criterions_CriterionId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Marks");

            migrationBuilder.RenameColumn(
                name: "CriterionId",
                table: "Marks",
                newName: "EvaluationId");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_CriterionId",
                table: "Marks",
                newName: "IX_Marks_EvaluationId");

            migrationBuilder.AddColumn<Guid>(
                name: "CriterionModelId",
                table: "Marks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_CriterionModelId",
                table: "Marks",
                column: "CriterionModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_CriterionEvaluationOptions_EvaluationId",
                table: "Marks",
                column: "EvaluationId",
                principalTable: "CriterionEvaluationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Criterions_CriterionModelId",
                table: "Marks",
                column: "CriterionModelId",
                principalTable: "Criterions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_CriterionEvaluationOptions_EvaluationId",
                table: "Marks");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Criterions_CriterionModelId",
                table: "Marks");

            migrationBuilder.DropIndex(
                name: "IX_Marks_CriterionModelId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "CriterionModelId",
                table: "Marks");

            migrationBuilder.RenameColumn(
                name: "EvaluationId",
                table: "Marks",
                newName: "CriterionId");

            migrationBuilder.RenameIndex(
                name: "IX_Marks_EvaluationId",
                table: "Marks",
                newName: "IX_Marks_CriterionId");

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Marks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Criterions_CriterionId",
                table: "Marks",
                column: "CriterionId",
                principalTable: "Criterions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
