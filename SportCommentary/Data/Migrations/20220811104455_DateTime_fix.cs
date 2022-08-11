using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportCommentary.Data.Migrations
{
    public partial class DateTime_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventID);
                });

            migrationBuilder.CreateTable(
                name: "SportType",
                columns: table => new
                {
                    SportTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportType", x => x.SportTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Commentary",
                columns: table => new
                {
                    CommentaryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentaryStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SportTypeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commentary", x => x.CommentaryID);
                    table.ForeignKey(
                        name: "FK_Commentary_SportType_SportTypeID",
                        column: x => x.SportTypeID,
                        principalTable: "SportType",
                        principalColumn: "SportTypeID");
                });

            migrationBuilder.CreateTable(
                name: "SingleComment",
                columns: table => new
                {
                    SingleCommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentaryID = table.Column<int>(type: "int", nullable: false),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleComment", x => x.SingleCommentID);
                    table.ForeignKey(
                        name: "FK_SingleComment_Commentary_CommentaryID",
                        column: x => x.CommentaryID,
                        principalTable: "Commentary",
                        principalColumn: "CommentaryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SingleComment_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commentary_SportTypeID",
                table: "Commentary",
                column: "SportTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleComment_CommentaryID",
                table: "SingleComment",
                column: "CommentaryID");

            migrationBuilder.CreateIndex(
                name: "IX_SingleComment_EventID",
                table: "SingleComment",
                column: "EventID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SingleComment");

            migrationBuilder.DropTable(
                name: "Commentary");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "SportType");
        }
    }
}
