using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class projectPolicies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "ProjectPolicies" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    ModificationDate = table.Column<DateTime>(nullable : false) ,
                    Policy = table.Column<int>(nullable : false) ,
                    ProjectId = table.Column<int>(nullable : false) ,
                    SerializedPolicySetting = table.Column<string>(nullable : true)
                } ,
                constraints : table =>
                {
                    table.PrimaryKey("PK_ProjectPolicies" , x => x.Id);
                    table.ForeignKey(
                        "FK_ProjectPolicies_Project_ProjectId" ,
                        x => x.ProjectId ,
                        "Project" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_ProjectPolicies_ProjectId" ,
                "ProjectPolicies" ,
                "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "ProjectPolicies");
        }
    }
}