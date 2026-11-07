using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RaceDay.API.Data;

#nullable disable

namespace RaceDay.API.Migrations;

[DbContext(typeof(RaceDayContext))]
[Migration("20261024090000_AddEventImageUrl")]
public partial class AddEventImageUrl : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ImageUrl",
            table: "Events",
            type: "nvarchar(1000)",
            maxLength: 1000,
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "ImageUrl", table: "Events");
    }
}
