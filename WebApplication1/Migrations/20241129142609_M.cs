using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
        public partial class M : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Destinations_StartDestinationId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StartDestinationId",
                table: "Routes");

            migrationBuilder.AddColumn<int>(
                name: "EndDestinationDestinationId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EndDestinationId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_EndDestinationDestinationId",
                table: "Routes",
                column: "EndDestinationDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_EndDestinationId",
                table: "Routes",
                column: "EndDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Destinations_EndDestinationDestinationId",
                table: "Routes",
                column: "EndDestinationDestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Destinations_EndDestinationId",
                table: "Routes",
                column: "EndDestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId");
        }

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Destinations_EndDestinationDestinationId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Destinations_EndDestinationId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_EndDestinationDestinationId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_EndDestinationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "EndDestinationDestinationId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "EndDestinationId",
                table: "Routes");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StartDestinationId",
                table: "Routes",
                column: "StartDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Destinations_StartDestinationId",
                table: "Routes",
                column: "StartDestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId");
        }
    }
}
