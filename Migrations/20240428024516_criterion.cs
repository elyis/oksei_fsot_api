using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class criterion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LowerBound",
                table: "Criterions");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Criterions");

            migrationBuilder.DropColumn(
                name: "UpperBound",
                table: "Criterions");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Users",
                newName: "TokenValidBefore");

            migrationBuilder.CreateTable(
                name: "CriterionEvaluationOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CountPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    CriterionId = table.Column<Guid>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_CriterionEvaluationOptions_CriterionId",
                table: "CriterionEvaluationOptions",
                column: "CriterionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriterionEvaluationOptions");

            migrationBuilder.RenameColumn(
                name: "TokenValidBefore",
                table: "Users",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<int>(
                name: "LowerBound",
                table: "Criterions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "Criterions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpperBound",
                table: "Criterions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
