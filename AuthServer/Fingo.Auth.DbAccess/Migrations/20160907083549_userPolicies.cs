using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class userPolicies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedPolicySetting",
                table: "ProjectPolicies");

            migrationBuilder.CreateTable(
                name: "UserPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ProjectPoliciesId = table.Column<int>(nullable: false),
                    SerializedUserPolicySetting = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPolicies_ProjectPolicies_ProjectPoliciesId",
                        column: x => x.ProjectPoliciesId,
                        principalTable: "ProjectPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPolicies_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
                name: "SerializedProjectPolicySetting",
                table: "ProjectPolicies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPolicies_ProjectPoliciesId",
                table: "UserPolicies",
                column: "ProjectPoliciesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPolicies_UserId",
                table: "UserPolicies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedProjectPolicySetting",
                table: "ProjectPolicies");

            migrationBuilder.DropTable(
                name: "UserPolicies");

            migrationBuilder.AddColumn<string>(
                name: "SerializedPolicySetting",
                table: "ProjectPolicies",
                nullable: true);
        }
    }
}
