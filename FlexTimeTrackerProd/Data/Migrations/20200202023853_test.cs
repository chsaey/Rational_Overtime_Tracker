using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlexTimeTrackerProd.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Shift",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AddEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Minutes = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddEntry", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UseEntry",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Minutes = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseEntry", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserMinutes",
                columns: table => new
                {
                    ApplicationUserID = table.Column<string>(nullable: false),
                    Minutes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMinutes", x => x.ApplicationUserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddEntry");

            migrationBuilder.DropTable(
                name: "UseEntry");

            migrationBuilder.DropTable(
                name: "UserMinutes");

            migrationBuilder.DropColumn(
                name: "Shift",
                table: "AspNetUsers");
        }
    }
}
