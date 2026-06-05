using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountantHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobDeadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.Sql(
                """UPDATE "Jobs" SET "Deadline" = "CreatedAt" + INTERVAL '30 days' WHERE "Deadline" IS NULL;""");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "Jobs",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Jobs");
        }
    }
}
