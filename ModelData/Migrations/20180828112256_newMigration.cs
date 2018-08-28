using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ModelData.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientListModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientListModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameListModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Developer = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Format = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    ReleaseDateEU = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    ReleaseDateJP = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    ReleaseDateNA = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameListModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientListModel");

            migrationBuilder.DropTable(
                name: "GameListModel");
        }
    }
}
