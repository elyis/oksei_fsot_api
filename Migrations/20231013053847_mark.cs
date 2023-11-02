using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace oksei_fsot_api.Migrations
{
    /// <inheritdoc />
    public partial class mark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluatedAppraiserModel_Users_AppraiserId",
                table: "EvaluatedAppraiserModel");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluatedAppraiserModel_Users_EvaluatedId",
                table: "EvaluatedAppraiserModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_EvaluatedAppraiserModel_EvaluatedAppraiserEvaluatedId_EvaluatedAppraiserAppraiserId",
                table: "Marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvaluatedAppraiserModel",
                table: "EvaluatedAppraiserModel");

            migrationBuilder.RenameTable(
                name: "EvaluatedAppraiserModel",
                newName: "EvaluatedAppraisers");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluatedAppraiserModel_AppraiserId",
                table: "EvaluatedAppraisers",
                newName: "IX_EvaluatedAppraisers_AppraiserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvaluatedAppraisers",
                table: "EvaluatedAppraisers",
                columns: new[] { "EvaluatedId", "AppraiserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluatedAppraisers_Users_AppraiserId",
                table: "EvaluatedAppraisers",
                column: "AppraiserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluatedAppraisers_Users_EvaluatedId",
                table: "EvaluatedAppraisers",
                column: "EvaluatedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_EvaluatedAppraisers_EvaluatedAppraiserEvaluatedId_EvaluatedAppraiserAppraiserId",
                table: "Marks",
                columns: new[] { "EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId" },
                principalTable: "EvaluatedAppraisers",
                principalColumns: new[] { "EvaluatedId", "AppraiserId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluatedAppraisers_Users_AppraiserId",
                table: "EvaluatedAppraisers");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluatedAppraisers_Users_EvaluatedId",
                table: "EvaluatedAppraisers");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_EvaluatedAppraisers_EvaluatedAppraiserEvaluatedId_EvaluatedAppraiserAppraiserId",
                table: "Marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EvaluatedAppraisers",
                table: "EvaluatedAppraisers");

            migrationBuilder.RenameTable(
                name: "EvaluatedAppraisers",
                newName: "EvaluatedAppraiserModel");

            migrationBuilder.RenameIndex(
                name: "IX_EvaluatedAppraisers_AppraiserId",
                table: "EvaluatedAppraiserModel",
                newName: "IX_EvaluatedAppraiserModel_AppraiserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvaluatedAppraiserModel",
                table: "EvaluatedAppraiserModel",
                columns: new[] { "EvaluatedId", "AppraiserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluatedAppraiserModel_Users_AppraiserId",
                table: "EvaluatedAppraiserModel",
                column: "AppraiserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluatedAppraiserModel_Users_EvaluatedId",
                table: "EvaluatedAppraiserModel",
                column: "EvaluatedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_EvaluatedAppraiserModel_EvaluatedAppraiserEvaluatedId_EvaluatedAppraiserAppraiserId",
                table: "Marks",
                columns: new[] { "EvaluatedAppraiserEvaluatedId", "EvaluatedAppraiserAppraiserId" },
                principalTable: "EvaluatedAppraiserModel",
                principalColumns: new[] { "EvaluatedId", "AppraiserId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
