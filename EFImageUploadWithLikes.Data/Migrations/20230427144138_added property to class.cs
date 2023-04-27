using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFImageUploadWithLikes.Data.Migrations
{
    public partial class addedpropertytoclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Images",
                newName: "FileName");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUploaded",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUploaded",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Images",
                newName: "FilePath");
        }
    }
}
