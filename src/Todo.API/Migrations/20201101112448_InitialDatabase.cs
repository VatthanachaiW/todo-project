using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Todo.API.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    descriptions = table.Column<string>(nullable: true),
                    status = table.Column<byte>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    is_delete = table.Column<bool>(nullable: false),
                    created_by = table.Column<string>(nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<string>(nullable: true),
                    deleted_on = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_todos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_todos");
        }
    }
}
