using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectDeliverablesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverableEvaluations_ProjectDeliverable_ProjectDeliverableId",
                table: "DeliverableEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDeliverable_DeliverableType_DeliverableTypeId",
                table: "ProjectDeliverable");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDeliverable_Projects_ProjectId",
                table: "ProjectDeliverable");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_ProjectDeliverable_DeliverableId",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectDeliverable",
                table: "ProjectDeliverable");

            migrationBuilder.RenameTable(
                name: "ProjectDeliverable",
                newName: "ProjectDeliverables");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDeliverable_ProjectId",
                table: "ProjectDeliverables",
                newName: "IX_ProjectDeliverables_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDeliverable_DeliverableTypeId",
                table: "ProjectDeliverables",
                newName: "IX_ProjectDeliverables_DeliverableTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectDeliverables",
                table: "ProjectDeliverables",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverableEvaluations_ProjectDeliverables_ProjectDeliverableId",
                table: "DeliverableEvaluations",
                column: "ProjectDeliverableId",
                principalTable: "ProjectDeliverables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDeliverables_DeliverableType_DeliverableTypeId",
                table: "ProjectDeliverables",
                column: "DeliverableTypeId",
                principalTable: "DeliverableType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDeliverables_Projects_ProjectId",
                table: "ProjectDeliverables",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_ProjectDeliverables_DeliverableId",
                table: "Submissions",
                column: "DeliverableId",
                principalTable: "ProjectDeliverables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliverableEvaluations_ProjectDeliverables_ProjectDeliverableId",
                table: "DeliverableEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDeliverables_DeliverableType_DeliverableTypeId",
                table: "ProjectDeliverables");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDeliverables_Projects_ProjectId",
                table: "ProjectDeliverables");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_ProjectDeliverables_DeliverableId",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectDeliverables",
                table: "ProjectDeliverables");

            migrationBuilder.RenameTable(
                name: "ProjectDeliverables",
                newName: "ProjectDeliverable");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDeliverables_ProjectId",
                table: "ProjectDeliverable",
                newName: "IX_ProjectDeliverable_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDeliverables_DeliverableTypeId",
                table: "ProjectDeliverable",
                newName: "IX_ProjectDeliverable_DeliverableTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectDeliverable",
                table: "ProjectDeliverable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliverableEvaluations_ProjectDeliverable_ProjectDeliverableId",
                table: "DeliverableEvaluations",
                column: "ProjectDeliverableId",
                principalTable: "ProjectDeliverable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDeliverable_DeliverableType_DeliverableTypeId",
                table: "ProjectDeliverable",
                column: "DeliverableTypeId",
                principalTable: "DeliverableType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDeliverable_Projects_ProjectId",
                table: "ProjectDeliverable",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_ProjectDeliverable_DeliverableId",
                table: "Submissions",
                column: "DeliverableId",
                principalTable: "ProjectDeliverable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
