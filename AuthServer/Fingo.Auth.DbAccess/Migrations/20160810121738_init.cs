using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Project" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    ModificationDate = table.Column<DateTime>(nullable : false) ,
                    Name = table.Column<string>(maxLength : 30 , nullable : false) ,
                    ProjectGuid = table.Column<Guid>(nullable : false) ,
                    Status = table.Column<int>(nullable : false)
                } ,
                constraints : table => { table.PrimaryKey("PK_Project" , x => x.Id); });

            migrationBuilder.CreateTable(
                "User" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    ActivationToken = table.Column<string>(nullable : true) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    FirstName = table.Column<string>(maxLength : 30 , nullable : true) ,
                    LastName = table.Column<string>(maxLength : 30 , nullable : true) ,
                    Login = table.Column<string>(nullable : false) ,
                    ModificationDate = table.Column<DateTime>(nullable : false) ,
                    Password = table.Column<string>(nullable : false) ,
                    Status = table.Column<int>(nullable : false)
                } ,
                constraints : table => { table.PrimaryKey("PK_User" , x => x.Id); });

            migrationBuilder.CreateTable(
                "ClientInformation" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    ContactData = table.Column<string>(maxLength : 200 , nullable : true) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    ModificationDate = table.Column<DateTime>(nullable : false) ,
                    Name = table.Column<string>(maxLength : 100 , nullable : true) ,
                    ProjectIdFk = table.Column<int>(nullable : false)
                } ,
                constraints : table =>
                {
                    table.PrimaryKey("PK_ClientInformation" , x => x.Id);
                    table.ForeignKey(
                        "FK_ClientInformation_Project_ProjectIdFk" ,
                        x => x.ProjectIdFk ,
                        "Project" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ProjectUser" ,
                table => new
                {
                    ProjectId = table.Column<int>(nullable : false) ,
                    UserId = table.Column<int>(nullable : false)
                } ,
                constraints : table =>
                {
                    table.PrimaryKey("PK_ProjectUser" , x => new {x.ProjectId , x.UserId});
                    table.ForeignKey(
                        "FK_ProjectUser_Project_ProjectId" ,
                        x => x.ProjectId ,
                        "Project" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_ProjectUser_User_UserId" ,
                        x => x.UserId ,
                        "User" ,
                        "Id" ,
                        onDelete : ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_ClientInformation_ProjectIdFk" ,
                "ClientInformation" ,
                "ProjectIdFk" ,
                unique : true);

            migrationBuilder.CreateIndex(
                "IX_ProjectUser_ProjectId" ,
                "ProjectUser" ,
                "ProjectId");

            migrationBuilder.CreateIndex(
                "IX_ProjectUser_UserId" ,
                "ProjectUser" ,
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "ClientInformation");

            migrationBuilder.DropTable(
                "ProjectUser");

            migrationBuilder.DropTable(
                "Project");

            migrationBuilder.DropTable(
                "User");
        }
    }
}