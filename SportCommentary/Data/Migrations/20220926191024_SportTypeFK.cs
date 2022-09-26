using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportCommentary.Data.Migrations
{
    public partial class SportTypeFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentary_SportType_SportTypeID",
                table: "Commentary");

            migrationBuilder.AlterColumn<int>(
                name: "SportTypeID",
                table: "Commentary",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commentary_SportType_SportTypeID",
                table: "Commentary",
                column: "SportTypeID",
                principalTable: "SportType",
                principalColumn: "SportTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentary_SportType_SportTypeID",
                table: "Commentary");

            migrationBuilder.AlterColumn<int>(
                name: "SportTypeID",
                table: "Commentary",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentary_SportType_SportTypeID",
                table: "Commentary",
                column: "SportTypeID",
                principalTable: "SportType",
                principalColumn: "SportTypeID");
        }
    }
}
