using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class AgregarColumnas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "cursoInstructor",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "curso",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDeCreacion",
                table: "comentario",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "cursoInstructor");

            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "curso");

            migrationBuilder.DropColumn(
                name: "FechaDeCreacion",
                table: "comentario");
        }
    }
}
