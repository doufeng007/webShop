using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Migrations
{
    public partial class workflowtemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowModelColumn_WorkFlowModel_WorkFlowModelId",
                table: "WorkFlowModelColumn");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkFlowTemplate_WorkFlowModel_WorkFlowModelId",
                table: "WorkFlowTemplate");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowTemplate_WorkFlowModelId",
                table: "WorkFlowTemplate");

            migrationBuilder.DropIndex(
                name: "IX_WorkFlowModelColumn_WorkFlowModelId",
                table: "WorkFlowModelColumn");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFlowModelId",
                table: "WorkFlowTemplate",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFlowModelId",
                table: "WorkFlowModelColumn",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFlowModelId",
                table: "WorkFlowTemplate",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkFlowModelId",
                table: "WorkFlowModelColumn",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplate_WorkFlowModelId",
                table: "WorkFlowTemplate",
                column: "WorkFlowModelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowModelColumn_WorkFlowModelId",
                table: "WorkFlowModelColumn",
                column: "WorkFlowModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowModelColumn_WorkFlowModel_WorkFlowModelId",
                table: "WorkFlowModelColumn",
                column: "WorkFlowModelId",
                principalTable: "WorkFlowModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkFlowTemplate_WorkFlowModel_WorkFlowModelId",
                table: "WorkFlowTemplate",
                column: "WorkFlowModelId",
                principalTable: "WorkFlowModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
