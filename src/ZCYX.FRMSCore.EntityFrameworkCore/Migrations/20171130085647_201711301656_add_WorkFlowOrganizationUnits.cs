using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Migrations
{
    public partial class _201711301656_add_WorkFlowOrganizationUnits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpOrganizationUnits",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChargeLeader",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildsLength",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Depth",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Leader",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AbpOrganizationUnits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "ChargeLeader",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "ChildsLength",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Leader",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AbpOrganizationUnits");
        }
    }
}
