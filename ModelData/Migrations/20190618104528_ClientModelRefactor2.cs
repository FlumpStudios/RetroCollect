using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ModelData.Migrations
{
    public partial class ClientModelRefactor2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFormat_Game_GameId",
                table: "GameFormat");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropIndex(
                name: "IX_GameFormat_GameId",
                table: "GameFormat");

            migrationBuilder.AddColumn<int>(
                name: "ClientListModelClientId",
                table: "GameFormat",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameFormat_ClientListModelClientId",
                table: "GameFormat",
                column: "ClientListModelClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFormat_ClientListModel_ClientListModelClientId",
                table: "GameFormat",
                column: "ClientListModelClientId",
                principalTable: "ClientListModel",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFormat_ClientListModel_ClientListModelClientId",
                table: "GameFormat");

            migrationBuilder.DropIndex(
                name: "IX_GameFormat_ClientListModelClientId",
                table: "GameFormat");

            migrationBuilder.DropColumn(
                name: "ClientListModelClientId",
                table: "GameFormat");

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Game_ClientListModel_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ClientListModel",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameFormat_GameId",
                table: "GameFormat",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ClientId",
                table: "Game",
                column: "ClientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameFormat_Game_GameId",
                table: "GameFormat",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
