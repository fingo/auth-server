using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class customData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "ProjectCustomData" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    ConfigurationName = table.Column<string>(nullable : true) ,
                    ConfigurationType = table.Column<int>(nullable : false) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    ModificationDate = table.Column<DateTime>(nullable : false) ,
                    ProjectId = table.Column<int>(nullable : false) ,
                    SerializedConfiguration = table.Column<string>(nullable : true)
                } ,
                constraints : table =>
                {
                    table.PrimaryKey("PK_ProjectCustomData" , x => x.Id);
                    table.ForeignKey(
                        "FK_ProjectCustomData_Project_ProjectId" ,
                        x => x.ProjectId ,
                        "Project" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "UserCustomData" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    ModificationDate = table.Column<DateTime>(nullable : false) ,
                    ProjectCustomDataId = table.Column<int>(nullable : false) ,
                    SerializedConfiguration = table.Column<string>(nullable : true) ,
                    UserId = table.Column<int>(nullable : false)
                } ,
                constraints : table =>
                {
                    table.PrimaryKey("PK_UserCustomData" , x => x.Id);
                    table.ForeignKey(
                        "FK_UserCustomData_ProjectCustomData_ProjectCustomDataId" ,
                        x => x.ProjectCustomDataId ,
                        "ProjectCustomData" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_UserCustomData_User_UserId" ,
                        x => x.UserId ,
                        "User" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_ProjectCustomData_ProjectId" ,
                "ProjectCustomData" ,
                "ProjectId");

            migrationBuilder.CreateIndex(
                "IX_UserCustomData_ProjectCustomDataId" ,
                "UserCustomData" ,
                "ProjectCustomDataId");

            migrationBuilder.CreateIndex(
                "IX_UserCustomData_UserId" ,
                "UserCustomData" ,
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "UserCustomData");

            migrationBuilder.DropTable(
                "ProjectCustomData");
        }
    }
}