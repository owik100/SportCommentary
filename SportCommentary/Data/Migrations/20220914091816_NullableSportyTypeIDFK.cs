using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportCommentary.Data.Migrations
{
    public partial class NullableSportyTypeIDFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_SportType_SportTypeID",
                table: "Event");

            migrationBuilder.AlterColumn<int>(
                name: "SportTypeID",
                table: "Event",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SportType_SportTypeID",
                table: "Event",
                column: "SportTypeID",
                principalTable: "SportType",
                principalColumn: "SportTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_SportType_SportTypeID",
                table: "Event");

            migrationBuilder.AlterColumn<int>(
                name: "SportTypeID",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_SportType_SportTypeID",
                table: "Event",
                column: "SportTypeID",
                principalTable: "SportType",
                principalColumn: "SportTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
