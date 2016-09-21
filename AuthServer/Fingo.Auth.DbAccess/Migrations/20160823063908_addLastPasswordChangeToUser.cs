using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fingo.Auth.DbAccess.Migrations
{
    public partial class addLastPasswordChangeToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                "LastPasswordChange" ,
                "User" ,
                nullable : false ,
                defaultValue : new DateTime(1 , 1 , 1 , 0 , 0 , 0 , 0 , DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "LastPasswordChange" ,
                "User");
        }
    }
}