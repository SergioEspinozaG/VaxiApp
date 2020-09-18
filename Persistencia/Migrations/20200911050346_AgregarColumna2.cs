using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class AgregarColumna2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "cursoInstructor");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "instructor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "instructor");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "cursoInstructor",
                type: "datetime2",
                nullable: true);
        }
    }
}
