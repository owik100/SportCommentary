using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportCommentary.Data.Migrations
{
    public partial class FK_Key_Event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SportTypeID",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Event_SportTypeID",
                table: "Event",
                column: "SportTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SportType_SportTypeID",
                table: "Event",
                column: "SportTypeID",
                principalTable: "SportType",
                principalColumn: "SportTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_SportType_SportTypeID",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_SportTypeID",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "SportTypeID",
                table: "Event");
        }
    }
}
