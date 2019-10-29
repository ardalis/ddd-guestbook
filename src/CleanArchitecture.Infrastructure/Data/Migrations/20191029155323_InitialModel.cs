using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchitecture.Infrastructure.Data.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guestbooks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guestbooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsDone = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuestbookEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailAddress = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    DateTimeCreated = table.Column<DateTimeOffset>(nullable: false),
                    GuestbookId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestbookEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestbookEntry_Guestbooks_GuestbookId",
                        column: x => x.GuestbookId,
                        principalTable: "Guestbooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuestbookEntry_GuestbookId",
                table: "GuestbookEntry",
                column: "GuestbookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuestbookEntry");

            migrationBuilder.DropTable(
                name: "ToDoItems");

            migrationBuilder.DropTable(
                name: "Guestbooks");
        }
    }
}
