using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class userPolicies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "SerializedPolicySetting" ,
                "ProjectPolicies");

            migrationBuilder.CreateTable(
                "UserPolicies" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    ModificationDate = table.Column<DateTime>(nullable : false) ,
                    ProjectPoliciesId = table.Column<int>(nullable : false) ,
                    SerializedUserPolicySetting = table.Column<string>(nullable : true) ,
                    UserId = table.Column<int>(nullable : false)
                } ,
                constraints : table =>
                {
                    table.PrimaryKey("PK_UserPolicies" , x => x.Id);
                    table.ForeignKey(
                        "FK_UserPolicies_ProjectPolicies_ProjectPoliciesId" ,
                        x => x.ProjectPoliciesId ,
                        "ProjectPolicies" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_UserPolicies_User_UserId" ,
                        x => x.UserId ,
                        "User" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
                "SerializedProjectPolicySetting" ,
                "ProjectPolicies" ,
                nullable : true);

            migrationBuilder.CreateIndex(
                "IX_UserPolicies_ProjectPoliciesId" ,
                "UserPolicies" ,
                "ProjectPoliciesId");

            migrationBuilder.CreateIndex(
                "IX_UserPolicies_UserId" ,
                "UserPolicies" ,
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "SerializedProjectPolicySetting" ,
                "ProjectPolicies");

            migrationBuilder.DropTable(
                "UserPolicies");

            migrationBuilder.AddColumn<string>(
                "SerializedPolicySetting" ,
                "ProjectPolicies" ,
                nullable : true);
        }
    }
}