using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class PriceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogEvent_Catalogs_CatalogsId",
                table: "CatalogEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogEvent_Events_EventsId",
                table: "CatalogEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProduct_Catalogs_CatalogsId",
                table: "CatalogProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CatalogProduct_Products_ProductsId",
                table: "CatalogProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStand_Events_EventsId",
                table: "EventStand");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStand_Stands_StandsId",
                table: "EventStand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventStand",
                table: "EventStand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogProduct",
                table: "CatalogProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CatalogEvent",
                table: "CatalogEvent");

            migrationBuilder.RenameTable(
                name: "EventStand",
                newName: "EventStands");

            migrationBuilder.RenameTable(
                name: "CatalogProduct",
                newName: "ProductCatalogs");

            migrationBuilder.RenameTable(
                name: "CatalogEvent",
                newName: "EventCatalogs");

            migrationBuilder.RenameIndex(
                name: "IX_EventStand_StandsId",
                table: "EventStands",
                newName: "IX_EventStands_StandsId");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogProduct_ProductsId",
                table: "ProductCatalogs",
                newName: "IX_ProductCatalogs_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_CatalogEvent_EventsId",
                table: "EventCatalogs",
                newName: "IX_EventCatalogs_EventsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventStands",
                table: "EventStands",
                columns: new[] { "EventsId", "StandsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCatalogs",
                table: "ProductCatalogs",
                columns: new[] { "CatalogsId", "ProductsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCatalogs",
                table: "EventCatalogs",
                columns: new[] { "CatalogsId", "EventsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventCatalogs_Catalogs_CatalogsId",
                table: "EventCatalogs",
                column: "CatalogsId",
                principalTable: "Catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventCatalogs_Events_EventsId",
                table: "EventCatalogs",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventStands_Events_EventsId",
                table: "EventStands",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventStands_Stands_StandsId",
                table: "EventStands",
                column: "StandsId",
                principalTable: "Stands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCatalogs_Catalogs_CatalogsId",
                table: "ProductCatalogs",
                column: "CatalogsId",
                principalTable: "Catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCatalogs_Products_ProductsId",
                table: "ProductCatalogs",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCatalogs_Catalogs_CatalogsId",
                table: "EventCatalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EventCatalogs_Events_EventsId",
                table: "EventCatalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStands_Events_EventsId",
                table: "EventStands");

            migrationBuilder.DropForeignKey(
                name: "FK_EventStands_Stands_StandsId",
                table: "EventStands");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCatalogs_Catalogs_CatalogsId",
                table: "ProductCatalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCatalogs_Products_ProductsId",
                table: "ProductCatalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCatalogs",
                table: "ProductCatalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventStands",
                table: "EventStands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCatalogs",
                table: "EventCatalogs");

            migrationBuilder.RenameTable(
                name: "ProductCatalogs",
                newName: "CatalogProduct");

            migrationBuilder.RenameTable(
                name: "EventStands",
                newName: "EventStand");

            migrationBuilder.RenameTable(
                name: "EventCatalogs",
                newName: "CatalogEvent");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCatalogs_ProductsId",
                table: "CatalogProduct",
                newName: "IX_CatalogProduct_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventStands_StandsId",
                table: "EventStand",
                newName: "IX_EventStand_StandsId");

            migrationBuilder.RenameIndex(
                name: "IX_EventCatalogs_EventsId",
                table: "CatalogEvent",
                newName: "IX_CatalogEvent_EventsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogProduct",
                table: "CatalogProduct",
                columns: new[] { "CatalogsId", "ProductsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventStand",
                table: "EventStand",
                columns: new[] { "EventsId", "StandsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CatalogEvent",
                table: "CatalogEvent",
                columns: new[] { "CatalogsId", "EventsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogEvent_Catalogs_CatalogsId",
                table: "CatalogEvent",
                column: "CatalogsId",
                principalTable: "Catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogEvent_Events_EventsId",
                table: "CatalogEvent",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProduct_Catalogs_CatalogsId",
                table: "CatalogProduct",
                column: "CatalogsId",
                principalTable: "Catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogProduct_Products_ProductsId",
                table: "CatalogProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventStand_Events_EventsId",
                table: "EventStand",
                column: "EventsId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventStand_Stands_StandsId",
                table: "EventStand",
                column: "StandsId",
                principalTable: "Stands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
