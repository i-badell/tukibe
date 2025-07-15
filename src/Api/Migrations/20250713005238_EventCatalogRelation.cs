using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class EventCatalogRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventCatalogs");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Catalogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_EventId",
                table: "Catalogs",
                column: "EventId",
                unique: true,
                filter: "[EventId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Events_EventId",
                table: "Catalogs",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Events_EventId",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_EventId",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Catalogs");

            migrationBuilder.CreateTable(
                name: "EventCatalogs",
                columns: table => new
                {
                    CatalogsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCatalogs", x => new { x.CatalogsId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_EventCatalogs_Catalogs_CatalogsId",
                        column: x => x.CatalogsId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventCatalogs_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventCatalogs_EventsId",
                table: "EventCatalogs",
                column: "EventsId");
        }
    }
}
