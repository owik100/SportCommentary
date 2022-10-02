using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportCommentary.Data.Migrations
{
    public partial class nullableEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleComment_Event_EventID",
                table: "SingleComment");

            migrationBuilder.AlterColumn<int>(
                name: "EventID",
                table: "SingleComment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleComment_Event_EventID",
                table: "SingleComment",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "EventID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SingleComment_Event_EventID",
                table: "SingleComment");

            migrationBuilder.AlterColumn<int>(
                name: "EventID",
                table: "SingleComment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleComment_Event_EventID",
                table: "SingleComment",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
