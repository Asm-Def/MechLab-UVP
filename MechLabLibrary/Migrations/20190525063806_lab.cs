using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MechLabLibrary.Migrations
{
    public partial class lab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Labs",
                columns: table => new
                {
                    LabID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ViewX = table.Column<double>(nullable: false),
                    ViewY = table.Column<double>(nullable: false),
                    Eyeshot = table.Column<double>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labs", x => x.LabID);
                });

            migrationBuilder.CreateTable(
                name: "Objects",
                columns: table => new
                {
                    LabID = table.Column<Guid>(nullable: false),
                    ObjectID = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    X = table.Column<double>(nullable: false),
                    Y = table.Column<double>(nullable: false),
                    VX = table.Column<double>(nullable: false),
                    VY = table.Column<double>(nullable: false),
                    M = table.Column<double>(nullable: false),
                    R = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objects", x => new { x.LabID, x.ObjectID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Labs");

            migrationBuilder.DropTable(
                name: "Objects");
        }
    }
}
