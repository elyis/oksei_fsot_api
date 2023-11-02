using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_CriterionModel_CriterionId",
                table: "Marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CriterionModel",
                table: "CriterionModel");

            migrationBuilder.RenameTable(
                name: "CriterionModel",
                newName: "Criterions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Criterions",
                table: "Criterions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Criterions_Name",
                table: "Criterions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Criterions_CriterionId",
                table: "Marks",
                column: "CriterionId",
                principalTable: "Criterions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Criterions_CriterionId",
                table: "Marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Criterions",
                table: "Criterions");

            migrationBuilder.DropIndex(
                name: "IX_Criterions_Name",
                table: "Criterions");

            migrationBuilder.RenameTable(
                name: "Criterions",
                newName: "CriterionModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CriterionModel",
                table: "CriterionModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_CriterionModel_CriterionId",
                table: "Marks",
                column: "CriterionId",
                principalTable: "CriterionModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
