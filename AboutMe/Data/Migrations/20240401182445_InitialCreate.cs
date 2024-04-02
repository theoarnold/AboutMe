using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AboutMe.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApplicationInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GithubCred = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GithubName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ButtonInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ButtonText = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    ButtonColourHex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ButtonUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationInfoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ButtonInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ButtonInfos_ApplicationInfos_ApplicationInfoId",
                        column: x => x.ApplicationInfoId,
                        principalTable: "ApplicationInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ApplicationInfos",
                columns: new[] { "Id", "Bio", "GithubCred", "GithubName" },
                values: new object[] { 1, "Log in to the dashboard to modify the biography section.", "This should be your GitHub credentials.", "This should be your GitHub username" });

            migrationBuilder.InsertData(
                table: "ButtonInfos",
                columns: new[] { "Id", "ApplicationInfoId", "ButtonColourHex", "ButtonText", "ButtonUrl" },
                values: new object[,]
                {
                    { 1, 1, "#ff0097", "Default Button", "https://github.com/theoarnold" },
                    { 2, 1, "#00ff00", "Another Button", "https://example.com" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ButtonInfos_ApplicationInfoId",
                table: "ButtonInfos",
                column: "ApplicationInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ButtonInfos");

            migrationBuilder.DropTable(
                name: "ApplicationInfos");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");
        }
    }
}
