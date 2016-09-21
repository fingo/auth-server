using Microsoft.EntityFrameworkCore.Migrations;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class setPasswordAddressInProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "SetPasswordAddress" ,
                "Project" ,
                nullable : false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "SetPasswordAddress" ,
                "Project");
        }
    }
}