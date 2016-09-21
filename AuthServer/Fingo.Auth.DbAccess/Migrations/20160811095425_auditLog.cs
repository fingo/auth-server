using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class auditLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AuditLog" ,
                table => new
                {
                    Id = table.Column<int>(nullable : false)
                        .Annotation("SqlServer:ValueGenerationStrategy" ,
                            SqlServerValueGenerationStrategy.IdentityColumn) ,
                    CreationDate = table.Column<DateTime>(nullable : false) ,
                    EventMassage = table.Column<string>(nullable : true) ,
                    EventType = table.Column<string>(nullable : true) ,
                    UserId = table.Column<int>(nullable : true)
                } ,
                constraints : table => { table.PrimaryKey("PK_AuditLog" , x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AuditLog");
        }
    }
}