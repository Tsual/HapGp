using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HapGp.Migrations
{
    public partial class db0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConfig",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfig", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BasicApplication",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicApplication", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "M_ProjectModels",
                columns: table => new
                {
                    Key = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    Subtitle = table.Column<string>(nullable: true),
                    TeacherID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_ProjectModels", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "M_ProjectSelectModels",
                columns: table => new
                {
                    Key = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectID = table.Column<int>(nullable: false),
                    StudentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_ProjectSelectModels", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "StorageModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TestModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserRecord",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: false),
                    LID = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecord", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EXT = table.Column<string>(nullable: true),
                    LID = table.Column<string>(nullable: false),
                    PWD = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConfig");

            migrationBuilder.DropTable(
                name: "BasicApplication");

            migrationBuilder.DropTable(
                name: "M_ProjectModels");

            migrationBuilder.DropTable(
                name: "M_ProjectSelectModels");

            migrationBuilder.DropTable(
                name: "StorageModel");

            migrationBuilder.DropTable(
                name: "TestModel");

            migrationBuilder.DropTable(
                name: "UserRecord");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
