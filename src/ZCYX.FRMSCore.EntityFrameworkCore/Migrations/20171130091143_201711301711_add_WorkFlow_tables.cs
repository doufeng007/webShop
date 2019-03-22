using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ZCYX.FRMSCore.Migrations
{
    public partial class _201711301711_add_WorkFlow_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLibrary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Height = table.Column<int>(nullable: false),
                    Ico = table.Column<string>(nullable: true),
                    Manager = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    OpenMode = table.Column<int>(nullable: false),
                    Params = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<Guid>(nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLibrary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlow ",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserID = table.Column<long>(nullable: false),
                    DesignJSON = table.Column<string>(nullable: true),
                    InstallDate = table.Column<DateTime>(nullable: false),
                    InstallUserID = table.Column<long>(nullable: false),
                    InstanceManager = table.Column<long>(nullable: false),
                    Manager = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RunJSON = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlow ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowForm ",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Attribute = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserID = table.Column<long>(nullable: false),
                    CreateUserName = table.Column<string>(nullable: true),
                    EventsJson = table.Column<string>(nullable: true),
                    Html = table.Column<string>(nullable: true),
                    LastModifyTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    SubTableJson = table.Column<string>(nullable: true),
                    Type = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowForm ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    CompletedTime = table.Column<DateTime>(nullable: true),
                    CompletedTime1 = table.Column<DateTime>(nullable: true),
                    Files = table.Column<string>(nullable: true),
                    FlowID = table.Column<Guid>(nullable: false),
                    GroupID = table.Column<Guid>(nullable: false),
                    InstanceID = table.Column<long>(nullable: false),
                    IsSign = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    OpenTime = table.Column<DateTime>(nullable: true),
                    OtherType = table.Column<int>(nullable: true),
                    PrevID = table.Column<Guid>(nullable: false),
                    PrevStepID = table.Column<Guid>(nullable: false),
                    ReceiveID = table.Column<long>(nullable: false),
                    ReceiveName = table.Column<string>(nullable: true),
                    ReceiveTime = table.Column<DateTime>(nullable: false),
                    SenderID = table.Column<long>(nullable: false),
                    SenderName = table.Column<string>(nullable: true),
                    SenderTime = table.Column<DateTime>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StepID = table.Column<Guid>(nullable: false),
                    StepName = table.Column<string>(nullable: true),
                    SubFlowGroupID = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TodoType = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowTask", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLibrary");

            migrationBuilder.DropTable(
                name: "WorkFlow ");

            migrationBuilder.DropTable(
                name: "WorkFlowForm ");

            migrationBuilder.DropTable(
                name: "WorkFlowTask");
        }
    }
}
