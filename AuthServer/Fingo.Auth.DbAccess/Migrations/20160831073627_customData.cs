using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class customData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCustomData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConfigurationName = table.Column<string>(nullable: true),
                    ConfigurationType = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    SerializedConfiguration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCustomData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectCustomData_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ProjectCustomDataId = table.Column<int>(nullable: false),
                    SerializedConfiguration = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCustomData_ProjectCustomData_ProjectCustomDataId",
                        column: x => x.ProjectCustomDataId,
                        principalTable: "ProjectCustomData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCustomData_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCustomData_ProjectId",
                table: "ProjectCustomData",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomData_ProjectCustomDataId",
                table: "UserCustomData",
                column: "ProjectCustomDataId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomData_UserId",
                table: "UserCustomData",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCustomData");

            migrationBuilder.DropTable(
                name: "ProjectCustomData");
        }
    }
}
